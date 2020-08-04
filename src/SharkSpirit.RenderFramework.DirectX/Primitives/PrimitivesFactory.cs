using System;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;

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
                    return CreateSolidSphere(device, configuration);
                    break;
                case PrimitiveDrawableTypes.Grid:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveDrawableTypes), primitiveDrawableTypes, null);
            }

            return new RenderObject(device);
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

        public static RenderObject CreateSolidSphere(IDevice device, IConfiguration configuration)
        {
            var sphereBuilder = new SpherePrimitiveBuilder(device, configuration);

            return sphereBuilder.Build(new Sphere.Sphere(device));
        }
    }
}
