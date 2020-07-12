using System.Numerics;

namespace SharpGPU {
    
    public static class Globals {
        public static int MaxAttributes { get; } = 16;
        public static int MaxUniforms { get; } = 16;
    }

    public enum AttType {
        Vec1, Vec2, Vec3, Vec4, None
    }

    public class Attribute {
        public float V1 { get; set; }
        public Vector2 V2 { get; set; }
        public Vector3 V3 { get; set; }
        public Vector4 V4 { get; set; }
        public AttType Type { get; set; } = AttType.None;
    }

    public enum UniType {
        Vec1, Vec2, Vec3, Vec4, Mat4, None
    }

    public class Uniform {
        public float V1 { get; set; }
        public Vector2 V2 { get; set; }
        public Vector3 V3 { get; set; }
        public Vector4 V4 { get; set; }
        public Matrix4x4 M4 { get; set; }
        public UniType Type { get; set; } = UniType.None;
    }

    public class Uniforms {
        public Uniform[] U { get; set; } = new Uniform[Globals.MaxUniforms];
    }

    public class InVertex {
        public int VertexId { get; set; } = -1;
        public Attribute[] Attributes { get; set; } = new Attribute[Globals.MaxAttributes];

    }

    public class OutVertex {
        public Vector4 Position { get; set; }
        public Attribute[] Attributes { get; set; } = new Attribute[Globals.MaxAttributes];

    }

    public class InFragment {
        public Vector4 Coordinate { get; set; }
        public Attribute[] Attributes { get; set; } = new Attribute[Globals.MaxAttributes];
    }

    public class OutFragment {
        public Vector4 Color;
    }
}