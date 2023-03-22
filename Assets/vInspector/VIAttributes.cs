using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VInspector
{
    public class ButtonAttribute : System.Attribute
    {
        public string name;

        public ButtonAttribute() => this.name = "";
        public ButtonAttribute(string name) => this.name = name;
    }




    public class TabAttribute : System.Attribute
    {
        public string name;

        public TabAttribute(string name) => this.name = name;
    }
    public class EndTabAttribute : System.Attribute { }



    public class FoldoutAttribute : System.Attribute
    {
        public string name;

        public FoldoutAttribute(string name) => this.name = name;
    }
    public class EndFoldoutAttribute : System.Attribute { }



    public class RangeAttribute : PropertyAttribute
    {
        public float min;
        public float max;

        public RangeAttribute(float min, float max) { this.min = min; this.max = max; }
    }


    public class SpaceAttribute : System.Attribute
    {
        public float space;

        public SpaceAttribute() => this.space = 10;
        public SpaceAttribute(float space) => this.space = space;
    }

    public class SizeAttribute : System.Attribute
    {
        public float size;

        public SizeAttribute(float size) => this.size = size;
    }

    public class VariantsAttribute : PropertyAttribute
    {
        public string[] variants;

        public VariantsAttribute(params string[] variants) => this.variants = variants;
    }
}