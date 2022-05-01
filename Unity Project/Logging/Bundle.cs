using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Bundle
{
    public interface ILoggableBundle
    {
        Bundle GetLogBundle(string prefix = "");
    }

    public static string GetPrefixHeader(string head, string pre)
    {
        bool gotprefix = pre != null && pre.Length > 0;
        bool gotheader = head != null && head.Length > 0;

        if (gotprefix && !gotheader)
            return pre;

        if (!gotprefix && gotheader)
            return head;

        if (gotprefix && gotheader)
            return pre + "." + head;

        return "";
    }

    public class HeaderContentPair
    {
        private string header;
        private object content;

        public HeaderContentPair(string header, object content)
        {
            this.header = header;
            this.content = content;
        }

        public void AddPrefix(string prefix)
        {
            header = GetPrefixHeader(header, prefix);
        }

        public string GetHeader(string prefix = "")
        {
            if (content is Bundle)
            {
                return ((Bundle)content).GetHeaders(GetPrefixHeader(header, prefix));
            }
            else
            {
                return GetPrefixHeader(header, prefix);
            }
        }

        public string GetContent()
        {
            if (content is Bundle)
            {
                return ((Bundle)content).GetContents();
            }
            else if (content is string)
            {
                return (string)content;
            }
            else
            {
                return content.ToString();
            }
        }
    }

    private string seperator = "\t";
    private string vectorPrecision = "F3";
    private string floatPrecision = "F3";
    private bool debug = false;
    private string prefix = "";

    public List<HeaderContentPair> loggablePairs = new List<HeaderContentPair>();

    public Bundle(string name = "", string passedPrefix = "")
    {
        this.prefix = GetPrefixHeader(passedPrefix, name);
    }

    private void AddPair(HeaderContentPair hcp)
    {
        hcp.AddPrefix(prefix);
        loggablePairs.Add(hcp);
    }

    public void Append(string header, Bundle sb)
    {
        AddPair(new HeaderContentPair(header, sb));
    }

    public void Append(Bundle sb)
    {
        AddPair(new HeaderContentPair("", sb));
    }

    public void Append(string header, string content)
    {
        AddPair(new HeaderContentPair(header, content));
    }


    public void Append(string header, object content)
    {
        AddPair(new HeaderContentPair(header, content.ToString()));
    }

    public void Append(string header, Vector2 content)
    {
        AddPair(new HeaderContentPair(header + ".x", content.x.ToString(vectorPrecision)));
        AddPair(new HeaderContentPair(header + ".y", content.y.ToString(vectorPrecision)));
    }

    public void Append(string header, Vector3 content)
    {
        AddPair(new HeaderContentPair(header + ".x", content.x.ToString(vectorPrecision)));
        AddPair(new HeaderContentPair(header + ".y", content.y.ToString(vectorPrecision)));
        AddPair(new HeaderContentPair(header + ".z", content.z.ToString(vectorPrecision)));
    }

    public string GetContents()
    {
        StringBuilder sb = new StringBuilder();
        loggablePairs.ForEach(x => {
            sb.Append(x.GetContent()); sb.Append(seperator);
        });
        return sb.ToString();
    }

    public string GetHeaders(string prefix = "")
    {
        StringBuilder sb = new StringBuilder();
        loggablePairs.ForEach(x => {
            sb.Append(x.GetHeader(prefix)); sb.Append(seperator);
        });
        return sb.ToString();
    }

    public void Append(string header, float content)
    {
        this.Append(header, content.ToString(floatPrecision));
    }

    public void logGameObject(string header, GameObject obj, bool localPosition = true, bool position = true, bool rotation = true, bool localRotation = true)
    {

        if (localPosition)
            Append(header + ".localposition", obj.transform.localPosition);

        if (position)
            Append(header + ".position", obj.transform.position);

        if (rotation)
            Append(header + ".rotation", obj.transform.rotation.eulerAngles);

        if (localRotation)
            Append(header + ".localrotation", obj.transform.localRotation.eulerAngles);
    }

    public void logGameObjectPivot(string header, GameObject obj, float pivotAngle, Vector3 pivotPoint, bool localPosition = true, bool position = true, bool rotation = true, bool localRotation = true)
    {
        if (position)
            Append(header + ".position.normalized", obj.transform.position.RotateAroundPivot(pivotPoint, new Vector3(0, pivotAngle, 0)));

        if (localPosition)
            Append(header + ".localposition.normalized", obj.transform.localPosition.RotateAroundPivot(pivotPoint, new Vector3(0, pivotAngle, 0)));

        if (rotation)
            Append(header + ".rotation.normalized", (obj.transform.rotation.eulerAngles - new Vector3(0, pivotAngle, 0)));

        if (localRotation)
            Append(header + ".localrotation.normalized", (obj.transform.localRotation.eulerAngles - new Vector3(0, pivotAngle, 0)));
    }

    public void logVectorPivot(string header, Vector3 obj, float pivotAngle, Vector3 pivotPoint, bool localPosition = true, bool position = true, bool rotation = true, bool localRotation = true)
    {
        if (position)
            Append(header + ".position.normalized", obj.RotateAroundPivot(pivotPoint, new Vector3(0, pivotAngle, 0)));
    }
}