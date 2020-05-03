using System;
using SharkSpirit.Core;

namespace SharkSpirit.RenderFramework.DirectX.Primitives
{
    public static class PrimitivesFactory
    {
        public static RenderObject Create(IDevice device, IConfiguration configuration, PrimitiveDrawableTypes primitiveDrawableTypes)
        {
            switch (primitiveDrawableTypes)
            {
                case PrimitiveDrawableTypes.Cube:
                    return CreateCube(device, configuration);
                case PrimitiveDrawableTypes.TexturedCube:
                    return CreateTexturedCube(device, configuration);
                case PrimitiveDrawableTypes.Sphere:
                    break;
                case PrimitiveDrawableTypes.Grid:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveDrawableTypes), primitiveDrawableTypes, null);
            }

            return new RenderObject(device, MeshType.None);
        }

        public static RenderObject CreateCube(IDevice device, IConfiguration configuration)
        {
            var cubeBuilder = new CubePrimitiveBuilder(device, configuration);

            return cubeBuilder.Build(new Cube(device));
        }

        public static RenderObject CreateTexturedCube(IDevice device, IConfiguration configuration)
        {
            var cubeBuilder = new TexturedCubePrimitiveBuilder(device, configuration);

            return cubeBuilder.Build(new Cube(device));
        }
    }
}
