using TMPro;
using UnityEngine;

namespace Controller.UI
{
    public class PhantomBodyController : MonoBehaviour
    {
        public TMP_InputField Xpos
        {
            get { return xpos; }
            set { xpos = value; }
        }

        [SerializeField] private TMP_InputField xpos;

        public TMP_InputField Ypos
        {
            get { return ypos; }
            set { ypos = value; }
        }

        [SerializeField] private TMP_InputField ypos;

        public TMP_InputField Zpos
        {
            get { return zpos; }
            set { zpos = value; }
        }

        [SerializeField] private TMP_InputField zpos;

        public TMP_InputField Size
        {
            get { return size; }
            set { size = value; }
        }

        [SerializeField] private TMP_InputField size;

        public Component BodyPrefab
        {
            get { return bodyPrefab; }
            set { bodyPrefab = value; }
        }

        [SerializeField] private Component bodyPrefab;

        private void OnEnable()
        {
            bodyPrefab.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            bodyPrefab.gameObject.SetActive(false);
        }

        private void Start()
        {
            xpos.onValueChanged.AddListener(delegate { updatePhantom(); });
            ypos.onValueChanged.AddListener(delegate { updatePhantom(); });
            zpos.onValueChanged.AddListener(delegate { updatePhantom(); });
            size.onValueChanged.AddListener(delegate { updatePhantom(); });
            updatePhantom();
        }

        private void updatePhantom()
        {
            Vector3 pos = scalePos(new Vector3(float.Parse(xpos.text), float.Parse(ypos.text), float.Parse(zpos.text)));
            bodyPrefab.transform.position = pos;
            bodyPrefab.transform.localScale = Vector3.one * scaleSize(float.Parse(size.text));
        }

        private Vector3 scalePos(Vector3 pos)
        {
            return pos*10;
        }
        private float scaleSize(float size)
        {
            return size;
        }
    }
}