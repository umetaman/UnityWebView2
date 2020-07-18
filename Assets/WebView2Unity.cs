using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WebView2Unity
{
    public class WebView2Unity : MonoBehaviour
    {
        public Image ImageRef;
        public Button ButtonRef;
        public Text ButtonTextRef;

        // Start is called before the first frame update
        void Start()
        {
            WebView2Native.Create("https://github.com/umetaman");
            StartCoroutine(delayNavigate());

            ButtonRef.onClick.AddListener(OnPressButton);
        }

        // Update is called once per frame
        void Update()
        {
            WebView2Native.UpdateBound(UIHelper.GetCanvasRectangle(ImageRef.rectTransform));
        }

        private void OnApplicationQuit()
        {
            if (WebView2Native.IsActive())
            {
                WebView2Native.Close();
            }
        }

        private void OnDestroy()
        {
            if (WebView2Native.IsActive())
            {
                WebView2Native.Close();
            }
        }

        private void OnDisable()
        {
            if (WebView2Native.IsActive())
            {
                WebView2Native.Close();
            }
        }

        private void OnPressButton()
        {
            if (WebView2Native.IsActive())
            {
                OnPressCloseButton();
            }
            else
            {
                OnPressOpenButton();
            }
        }

        private void OnPressCloseButton()
        {
            WebView2Native.Close();
            ButtonTextRef.text = "Open";
        }

        private void OnPressOpenButton()
        {
            WebView2Native.Create("https://google.co.jp");
            WebView2Native.UpdateBound(UIHelper.GetCanvasRectangle(ImageRef.rectTransform));

            ButtonTextRef.text = "Close";
        }

        private IEnumerator delayNavigate()
        {
            yield return new WaitForSeconds(1.0f);
            WebView2Native.NavigateToHTML("<html><body><h1>Hello, WebView2!</h1></body></html>");
        }
    }
}