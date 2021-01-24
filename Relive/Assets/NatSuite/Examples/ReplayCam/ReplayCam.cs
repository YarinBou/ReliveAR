/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using System.Collections;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;
    using System.Linq;
    using UnityEngine.SceneManagement;
    using NatSuite.Sharing;
    using UnityEngine.UI;
    using System;

    public class ReplayCam : MonoBehaviour {

        public AudioSource onSound;
        public AudioSource offSound;
        [Header("Recording Button")]
        public GameObject startRecordBtn;
        public GameObject stoprecordBtn;
        public Text timeCounter;
        private TimeSpan timePlaying;
        private bool timerGoing;
        private float elapsedTime;
        public Animator TimerOn;
        public Animator TimerOff;
        public GameObject TimerOffObj;
        public GameObject TimerOnObj;
        public Text timeCounter2;
        

        [Header("Recording")]
        public int videoWidth = 1920;
        public int videoHeight = 1080;
        public bool recordMicrophone;

        private IMediaRecorder recorder;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        private AudioSource microphoneSource;
        

        private int minFreq;
        private int maxFreq;

        private AudioSource goAudioSource;

        private IEnumerator Start () {

            recorder = null;
            cameraInput = null;
            audioInput = null;
            microphoneSource = null;

            timerGoing = false;
            foreach (var device in Microphone.devices)
            {
                Debug.Log("Name: " + device);
                
            }
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            Debug.Log("minFreq:" + minFreq);
            Debug.Log("maxFreq:" + maxFreq);

            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
            }

            


            startRecordBtn.SetActive(true);
            stoprecordBtn.SetActive(false);
            // Start microphone
            microphoneSource = this.GetComponent<AudioSource>();
            microphoneSource.mute =
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            microphoneSource.clip = Microphone.Start(null, true, 1, maxFreq);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();

        }

        private void OnDestroy () {
            // Stop microphone
            
            Microphone.End(null);
            microphoneSource.Stop();
            

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

            if(Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                videoHeight = 1080;
                videoWidth = 1920;
            }

            if(Screen.orientation == ScreenOrientation.Portrait)
            {
                videoHeight = 1920;
                videoWidth = 1080;
            }
        }

        public void StartRecording () {

            onSound.Play();
            startRecordBtn.SetActive(false);
            stoprecordBtn.SetActive(true);
            TimerOnObj.SetActive(true);
            TimerOn.enabled = true;
            TimerOff.enabled = false;
            TimerOffObj.SetActive(false);
            StartCoroutine(first(1.5F));
            
        }


        public async void StopRecording () {

            offSound.Play();
            TimerOffObj.SetActive(true);
            TimerOnObj.SetActive(false);
            TimerOff.enabled = true;
            TimerOn.enabled = false;

            Toast.Instance.Show("הסרטון נשמר בגלריה", 2);
            startRecordBtn.SetActive(true);
            stoprecordBtn.SetActive(false);
            // Mute microphone
            microphoneSource.mute = true;
            // Stop recording
            audioInput.Dispose();
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            
            // Playback recording
            Debug.Log($"Saved recording to: {path}");
            //var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
            //Handheld.PlayFullScreenMovie($"{prefix}{path}");
            //new NativeShare().AddFile(path).SetSubject("Subject goes here").SetText("Hello world!").Share();


            await new SavePayload().AddText("ReliveAR").AddMedia(path).Commit();

            StartCoroutine(Start());
            //StartCoroutine(loadscene(1.5F));


            
        }

        IEnumerator first(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            

            // Start recording
            var frameRate = 30;
            var sampleRate = AudioSettings.outputSampleRate;
            var channelCount = (int)AudioSettings.speakerMode;
            var clock = new RealtimeClock();
            
            recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount);
            // Create recording inputs
            cameraInput = new CameraInput(recorder, clock, Camera.main);
            audioInput = new AudioInput(recorder, clock, microphoneSource, true);
            // Unmute microphone
            microphoneSource.mute = audioInput == null;

        }

        IEnumerator loadscene(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void BeginTimer()
        {
            
            timerGoing = true;
            elapsedTime = 0f;

            StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while(timerGoing)
            {
                elapsedTime += Time.deltaTime;
                timePlaying = TimeSpan.FromSeconds(elapsedTime);
                string timeText = timePlaying.ToString("m':'ss");
                timeCounter.text = timeText;
                timeCounter2.text = timeText;

                yield return null;
            }
        }

        public void endTimer()
        {
            timerGoing = false;
        }
    }
}