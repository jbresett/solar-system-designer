using Model.Util;
using TMPro;
using UnityEngine;
using Util;

namespace Controller.UI
{
    public class PhantomBodyController : MonoBehaviour
    {
        private const float POSITION_MULT = 30492.3f;
        private const double SIZE_MULT = .1;
        
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
        
        public TMP_Dropdown Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        [SerializeField] private TMP_Dropdown unit;

        private UnitType unitScale = UnitType.Absolute;

        private void OnEnable()
        {
            bodyPrefab.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            try
            {
                bodyPrefab.gameObject.SetActive(false);
            } catch (MissingReferenceException)
            {
                // Ignore: Caused when de-activating during shutdown.
            }
        }

        private void Start()
        {
            xpos.onValueChanged.AddListener(delegate { updatePhantom(); });
            ypos.onValueChanged.AddListener(delegate { updatePhantom(); });
            zpos.onValueChanged.AddListener(delegate { updatePhantom(); });
            size.onValueChanged.AddListener(delegate { updatePhantom(); });
            unit.onValueChanged.AddListener(delegate { updateScale(); });
            updateScale();
        }

        private void updateScale()
        {
            UnitType newUnit = UnitConverter.unitTypes[unit.options[unit.value].text.ToLower()];
            unitScale = newUnit;
            updatePhantom();
        }
        private void updatePhantom()
        {
            Vector3 pos = scalePos(new Vector3(float.Parse(xpos.text.IfBlank("0")), 
                float.Parse(ypos.text.IfBlank("0")), float.Parse(zpos.text.IfBlank("0"))));
            bodyPrefab.transform.position = pos;
            bodyPrefab.transform.localScale = Vector3.one * (float)scaleSize(double.Parse(size.text.IfBlank("0")));
        }

        private Vector3 scalePos(Vector3 pos)
        {
            return pos*POSITION_MULT;
        }
        private double scaleSize(double size)
        {
            return UnitConverter.convertRadius(size,unitScale,UnitType.Earths)*SIZE_MULT;
        }
    }

}