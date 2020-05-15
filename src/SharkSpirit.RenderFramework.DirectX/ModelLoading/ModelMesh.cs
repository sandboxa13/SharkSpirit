using System;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace SharkSpirit.RenderFramework.DirectX.ModelLoading
{
    class ModelMesh: RenderObject
    {
        public ModelMesh(IDevice device) : base(device)
        {
        }
        
        InputElement[] m_inputElements;
        public InputElement[] InputElements
        {
            set { m_inputElements = value; }
            get { return m_inputElements; }
        }

        InputLayout m_inputLayout;
        public InputLayout InputLayout
        {
            set { m_inputLayout = value; }
            get { return m_inputLayout; }
        }

        int m_vertexSize;
        public int VertexSize
        {
            set { m_vertexSize = value; }
            get { return m_vertexSize; }
        }

        Buffer m_vertexBuffer;
        public Buffer VertexBuffer
        {
            set { m_vertexBuffer = value; }
            get { return m_vertexBuffer; }
        }

        Buffer m_indexBuffer;
        public Buffer IndexBuffer
        {
            set { m_indexBuffer = value; }
            get { return m_indexBuffer; }
        }

        int m_vertexCount;
        public int VertexCount
        {
            set { m_vertexCount = value; }
            get { return m_vertexCount; }
        }

        int m_indexCount;
        public int IndexCount
        {
            set { m_indexCount = value; }
            get { return m_indexCount; }
        }

        int m_primitiveCount;
        public int PrimitiveCount
        {
            set { m_primitiveCount = value; }
            get { return m_primitiveCount; }
        }

        PrimitiveTopology m_primitiveTopology;
        public PrimitiveTopology PrimitiveTopology
        {
            set { m_primitiveTopology = value; }
            get { return m_primitiveTopology; }
        }

        Texture2D m_diffuseTexture;
        public Texture2D DiffuseTexture
        {
            set { m_diffuseTexture = value; }
            get { return m_diffuseTexture; }
        }

        ShaderResourceView m_diffuseTextureView;
        public ShaderResourceView DiffuseTextureView
        {
            set { m_diffuseTextureView = value; }
            get { return m_diffuseTextureView; }
        }

        //add texture and texture view for the shader
        public void AddTextureDiffuse(Device device, string path)
        {
            // m_diffuseTexture = Texture2D.Fr<Texture2D>(device, path);
            m_diffuseTextureView = new ShaderResourceView(device, m_diffuseTexture);
        }

        //set the input layout and make sure it matches vertex format from the shader
        public void SetInputLayout(Device device, ShaderSignature inputSignature)
        {
            m_inputLayout = new InputLayout(device, inputSignature, m_inputElements);
            if (m_inputLayout == null)
            {
                throw new Exception("mesh and vertex shader input layouts do not match!");
            }
        }

        //dispose D3D related resources
        public void Dispose()
        {
            m_inputLayout.Dispose();
            m_vertexBuffer.Dispose();
            m_indexBuffer.Dispose();
        }

       
    }

}
