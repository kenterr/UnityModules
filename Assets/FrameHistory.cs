using UnityEngine;
using System.Collections;
using LeapInternal;
using Leap.Unity;
using Leap;

/** Stores the past Leap Motion tracking frames in a last-in, first-out (LIFO) 
* buffer.
*
* Note whether the first frame in the buffer is the
* current frame or the previous frame depends on the order which
* Unity executes the scripts. LeapProvider is set to update early in the script exection order.
* If the LeapProvider update or fixed update is called first, then
* Frames.Get(0) will return the same frame as LeapProvider.CurrentFrame. Otherwise, the 
* opposite is true. 
*
* When StoreUpdateFrames only is true, only tracking frames actually used in a Unity Update of 
* FixedUpdate call are stored. Since a Unity application typically runs at a slower frame rate than
* the Leap Motion service, this means that tracking frames will be skipped. However, turning off
* StoreUpdateFrames means that even unused frames must be transformed into the Unity coordinate space
* and stored in the buffer.
*/
public class FrameHistory : MonoBehaviour {
  private LeapProvider _provider;
  private Controller _controller;
  private long _lastAdded = 0;
  private LeapTransform _conversion = LeapTransform.Identity;

  [SerializeField] 
  private bool _storeUpdateFramesOnly = true;

  /** The LIFO queue of stored frames. */ 
  public CircularObjectBuffer<Frame> Frames { get; private set; }
  /** The number of frames to store. Older frames are discarded once this capacity is reached. */
  public int HistoryLength = 10;
  /** Whether to store only those frames actually used in the Unity Update or FixedUpdate calls.
  *
  * If false, all tracking frames are transformed into the Unity coordinate relative to the LeapHandController
  * and stored.
  */
  public bool StoreUpdateFramesOnly{
    get{ 
      return _storeUpdateFramesOnly; 
    }
    set{
      if(value != _storeUpdateFramesOnly){
        if(value == true){
          if(_controller == null){
                  _controller = new Controller();
          }
          _controller.FrameReady += storeServiceFrame;
        } else {
          _controller.FrameReady -= storeServiceFrame;
        }
        _storeUpdateFramesOnly = value;
      }
    }
  }

  void Start () {
    _provider = GetComponent<LeapProvider>();
    Frames = new CircularObjectBuffer<Frame>(HistoryLength);
  }

  void Update () {
    if(StoreUpdateFramesOnly){
      addFrame(_provider.CurrentFrame);
    } else {
      _conversion = transform.GetLeapMatrix();
    }
  }

  void FixedUpdate(){
    if(StoreUpdateFramesOnly){
      addFrame(_provider.CurrentFixedFrame);
    } else {
      _conversion = transform.GetLeapMatrix();
    }
  }

  void addFrame(Frame frame){
    if(frame.Id != _lastAdded){
      Frames.Put(frame);
      _lastAdded = frame.Id;
    }
  }

  void storeServiceFrame(object sender, FrameEventArgs frameEvent){
    addFrame(frameEvent.frame.TransformedCopy(_conversion)); 
  }
}
