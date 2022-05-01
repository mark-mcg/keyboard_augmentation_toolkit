using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using WindowsInput.Native;
using System.Runtime.Serialization;
using System.CodeDom;
using KAT.KeyCodeMappings;
using TriangleNet;

namespace KAT.Layouts
{
    [Serializable]
    public class KLEKeyboard
    {
        public KLEKeyboardMetadata meta;
        public List<KLEKey> keys;

        public override string ToString()
        {
            return "KLEKeyboard " + meta;
        }
    }

    [Serializable]
    public class KLEBackground
    {
        public string name;
        public string style;
    }

    [Serializable]
    public class KLEKeyboardMetadata
    {
        public string author;
        public string backcolor;
        public KLEBackground background;
        public string name;
        public string notes;
        public string radii;
        public string switchBrand;
        public string switchMount;
        public string switchType;

        public override string ToString()
        {
            return "metadata for layout created by " + author;
        }
    }

    [Serializable]
    public class KLEDefault
    {
        public string textColor;
        public float textSize;
    }

    [Serializable]
    public class KLEKey
    {
        public string color;
        public List<string> labels;
        public List<string> textColor;
        public List<float> textSize;
        public KLEDefault @default;
        public float x;
        public float y;
        public float width;
        public float height;
        public float number;
        public float x2;
        public float y2;
        public float width2;
        public float height2;
        public float number2;

        public float rotation_x;
        public float rotation_y;
        public float rotation_angle;
        public bool decal;
        public bool ghost;
        public bool stepped;
        public bool nub;

        public string profile;
        public string sm;
        public string sb;
        public string st;
    }

    public class KLELayoutParser { 

        public KLEKeyboard ParseLayoutFromAsset(TextAsset asset)
        {
            return ParseLayoutFromJSON(asset.text);
        }

        public KLEKeyboard ParseLayoutFromJSON(string text)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(text));
            return JsonSerializer.CreateDefault().Deserialize<KLEKeyboard>(reader);
        }

        public GameObject GenerateKeyboardLayoutFromKLEKeyboard(string name, KLEKeyboard keyboard)
        {
            GameObject root = new GameObject("KLEY Layout: " + name);

            int row = -1;
            int ind = -1;
            foreach (KLEKey key in keyboard.keys)
            {
                if (key.x == 0)
                {
                    row++;
                    ind = -1;
                }
                ind++;
                GameObject element = CreateKeyElement(root, key, row, ind);
            }

            return root;
        }

        private const float SizePerKeyUnit = 0.015f;

        public GameObject CreateKeyElement(GameObject root, KLEKey key, int row, int positionOnRow)
        {
            GameObject keyElement = new GameObject("KLEKey " + key);
            keyElement.transform.SetParent(root.transform);

            if (key.labels.Count == 0)
                key.labels.Add(" ");

            if (key.labels.Count >= 1)
            {
                key.height *= -1 * SizePerKeyUnit;
                key.y *= -1 * SizePerKeyUnit;
                key.width *= -1 * SizePerKeyUnit;
                key.x *= -1 * SizePerKeyUnit;
                key.height2 *= -1 * SizePerKeyUnit;
                key.width2 *= -1 * SizePerKeyUnit;
                key.x2 *= -1 * SizePerKeyUnit;
                key.y2 *= -1 * SizePerKeyUnit;

                Vector3 Dimensions = new Vector3(key.width, key.height, 0);
                Vector3 centerOffset = (Dimensions * 0.5f);
                Vector3 LocalPosition = new Vector3(key.x, key.y, 0) + centerOffset;


                Rect keyRect = new Rect(
                    // x, y, wid, hei
                    -centerOffset.x,
                    -centerOffset.y,
                    key.width,
                    key.height);

                List<Vector3> UISurfaceVertices = new List<Vector3>();
                List<Vector3> UpperKey = new List<Vector3>();

                UISurfaceVertices.AddRange(GenerateKeycapRectangularVertices(
                   keyRect, LocalPosition.y
                ));

                //Debug.Log("Key dims " + keyElement.Dimensions + " position " + keyElement.LocalPosition + " center offset " + centerOffset + " rect " + keyRect);

                if (key.width != key.width2 || key.height != key.height2)
                {
                    Vector3 upperKeyDims = new Vector3(key.width2, key.height2, 0);
                    Vector3 upperKeyPos = new Vector3(key.x2, key.y2, 0);
                    Vector3 upperKeyCenterOffset = (upperKeyDims * 0.5f);

                    Debug.Log("Upper Key dims " + upperKeyDims + " position " + upperKeyPos);

                    UpperKey.AddRange(GenerateKeycapRectangularVertices(
                        new Rect(
                            key.x2 - centerOffset.x, 
                            key.y2 - centerOffset.y, 
                            key.width2, 
                            key.height2), 
                        LocalPosition.y
                    ));
                    Debug.Log("Set upperkey to " + UpperKey.Count);
                }

                // now add the mesh to our game object, with appropriate position etc...
                UnityEngine.Mesh mesh;

                if (UpperKey.Count > 0)
                {
                    mesh = UnityMeshExtensions.MergeUnityMeshes(
                        new List<UnityEngine.Mesh>() { UnityMeshExtensions.CreateMeshFromVertices(UISurfaceVertices, true), UnityMeshExtensions.CreateMeshFromVertices(UpperKey, true) });
                } else
                {
                    mesh = UnityMeshExtensions.CreateMeshFromVertices(UISurfaceVertices, true);
                }

                keyElement.AddMesh(mesh, false, false, true, false);
                keyElement.transform.localPosition = LocalPosition;
                keyElement.transform.localEulerAngles = new Vector3(key.rotation_x, key.rotation_y, 0);// + new Vector3(0, 180, 0);
                keyElement.AddComponent<KUILocationBounds>();

                Color color;
                ColorUtility.TryParseHtmlString(key.color, out color);

                int labelPos = 0;
                List<Label> KeycapLabelling = new List<Label>();
                foreach (string label in key.labels)
                {
                    if (label != null && label != "null")
                    {
                        Label newLabel = new Label();
                        newLabel.text = label;
                        if (key.textColor.Count > labelPos)
                            ColorUtility.TryParseHtmlString(key.textColor[labelPos], out newLabel.color);
                        else
                            ColorUtility.TryParseHtmlString(key.@default.textColor, out newLabel.color);

                        if (key.textSize.Count > labelPos)
                            newLabel.textSize = key.textSize[labelPos];
                        else
                            newLabel.textSize = key.@default.textSize;

                        //Debug.Log("Labels are: " + string.Join(", ", key.labels));
                        if (key.labels.Count > 1)
                            newLabel.position = (Label.Position)labelPos;
                        else
                            newLabel.position = Label.Position.mm;
                        KeycapLabelling.Add(newLabel);
                    }
                    labelPos++;
                }


                string physicalKey = key.labels.Last();

                if (key.labels.Count > 1 && positionOnRow > 13)
                {
                    // may be on the numberpad with a numberpad key
                    // in which case first label is the primary
                    physicalKey = key.labels.First();
                }

                if (physicalKey != null)
                {
                    //Debug.Log("Parsing key " + physicalKey + " positionOnRow " + positionOnRow + " gives us a KKey of " + new KKey(KLEToKeyPosition.Forward(physicalKey, positionOnRow)));
                    KKey kkey = new KKey(KLEToKeyPosition.Forward(physicalKey, positionOnRow));
                    KUIKeyLocationMetadata locationTagMB = keyElement.AddComponent<KUIKeyLocationMetadata>();
                    locationTagMB.metadata.Add(kkey);
                }
            }
            return keyElement;

        }

        [Serializable]
        public class Label
        {
            public string text;
            public Color color;
            public Position position = Position.mm;
            public float textSize;
            public enum Position { tl, tm, tr, ml, mm, mr, bl, bm, br }
        }

        /// <summary>
        /// Use this if the key is regular (e.g. square/rectangular).
        /// N.B. This won't work if the key is intended to be irregular 
        /// e.g. L shaped enter keys
        /// </summary>
        public List<Vector3> GenerateKeycapRectangularVertices(Rect rectangle, float height)
        {
            return new List<Vector3>
                {
                    new Vector3(rectangle.xMin,rectangle.yMin, height),
                    new Vector3(rectangle.xMax,rectangle.yMin, height),
                    new Vector3(rectangle.xMax,rectangle.yMax,height),
                    new Vector3(rectangle.xMin,rectangle.yMax, height),

                    //new Vector3(0,0,0),
                    //new Vector3(0,0,1),
                    //new Vector3(-0.25f,0,1),
                    //new Vector3(-0.25f,0,2),
                    //new Vector3(2,0,2),
                    //new Vector3(2,0,0),
                };
        }
    }
}
