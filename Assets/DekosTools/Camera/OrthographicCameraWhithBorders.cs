/*
MIT License

Copyright (c) 2017 Rafael cosentino garcia

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//Orthographic Camera Whith Borders V0.1.0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DekosTools.Cameras
{
        [RequireComponent(typeof(Camera))]
        public class OrthographicCameraWhithBorders : MonoBehaviour
        {

            [SerializeField]
            Transform Target;
            [SerializeField]
            Vector3 OffSet = Vector3.forward;
            [SerializeField]
            float Speed;
            [SerializeField]
            ScreenBorders Borders;
            [Header("Zoom")]
            [SerializeField]
            float maxZoom;
            [SerializeField]
            float minZoom;
            public float ToZoom;
            [SerializeField]
            float zoomVel;
            [SerializeField]
            float startIntroZoom;
            [SerializeField]
            float endIntroZoom;

            /// <summary>
            /// For effects Only
            /// </summary>
            [HideInInspector]
            public Vector3 ExtraOffSet = Vector3.zero;

            void Awake()
            {
                this.GetComponent<Camera>().orthographic = true;
                this.transform.position = TrimCameraPosition(Target.transform.position, Borders) + OffSet;
                StartCoroutine("StartZoom");
            }


            IEnumerator StartZoom()
            {
                this.GetComponent<Camera>().orthographicSize = maxZoom;
                ToZoom = startIntroZoom;
                yield return new WaitForSeconds(1);
                ToZoom = endIntroZoom;
            }

            void FixedUpdate()
            {
                if (Target != null)
                {
                    Vector3 TargetPosition = TrimCameraPosition(Target.transform.position, Borders) + OffSet;
                    this.transform.position = Vector3.Lerp(
                        this.transform.position,
                        TargetPosition,
                        Time.fixedDeltaTime * Speed) + ExtraOffSet;
                }
                this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(this.GetComponent<Camera>().orthographicSize, ToZoom, zoomVel * Time.deltaTime);
            }

            Vector3 TrimCameraPosition(Vector3 _Positon, ScreenBorders _Borders)
            {
                float OrtSize = this.GetComponent<Camera>().orthographicSize;
                float CamSize = OrtSize * Screen.width / Screen.height;
                float Xmin = (float)(CamSize + _Borders.Xmin);
                float Xmax = (float)(_Borders.Xmax - CamSize);
                float Ymin = (float)(OrtSize + _Borders.Ymin);
                float Ymax = (float)(_Borders.Ymax - OrtSize);

                _Positon = new Vector3(
                    Mathf.Clamp(_Positon.x, Xmin, Xmax),
                    Mathf.Clamp(_Positon.y, Ymin, Ymax),
                    _Positon.z);
                return _Positon;
            }

            [System.Serializable]
            class ScreenBorders
            {
                [SerializeField]
                public float Xmin = 0;
                [SerializeField]
                public float Xmax = 0;

                [SerializeField]
                public float Ymin = 0;
                [SerializeField]
                public float Ymax = 0;
            }

            private void OnDrawGizmosSelected()
            {
                Vector3 Center = new Vector3((Borders.Xmax + Borders.Xmin)/2, (Borders.Ymax + Borders.Ymin) / 2, 0);
                Vector3 Size = new Vector3((Mathf.Abs(Borders.Xmax) + Mathf.Abs(Borders.Xmin)),
                    (Mathf.Abs(Borders.Ymax) + Mathf.Abs(Borders.Ymin)), 0);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(Center, Size);
            }
        }
}
