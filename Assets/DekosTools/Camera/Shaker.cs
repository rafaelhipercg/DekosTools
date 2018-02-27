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

//Camera Shaker V0.0.1

using UnityEngine;

namespace DekosTools.Cameras.Effects
{
    public class Shaker
    {

        [SerializeField]
        float time;
        [SerializeField]
        Vector3 range;
        [SerializeField]
        Vector3 frequency;

        /// <summary>
        /// Shake
        /// </summary>
        /// <param name="_time">time + Time.time</param>
        /// <param name="_range"></param>
        /// <param name="_frequency"></param>
        public Shaker(float _time, Vector3 _range, Vector3 _frequency)
        {
            time = _time;
            range = _range;
            frequency = _frequency;
        }

        public Vector3 GetShake()
        {
            float X = Mathf.Sin((Time.time) * frequency.x) * range.x;
            float Y = Mathf.Sin((Time.time) * frequency.y) * range.y;
            float Z = Mathf.Sin((Time.time) * frequency.z) * range.z;
            return Vector3.Slerp(new Vector3(X, Y, Z), Vector3.zero, (Time.time / time));
        }

    }
}
