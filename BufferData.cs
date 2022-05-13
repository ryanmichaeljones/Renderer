namespace Renderer
{
    public struct BufferData
    {
        public string name;
        public Buffer buffer;

        public BufferData(string name, Buffer buffer)
        {
            this.name = name;
            this.buffer = buffer;
        }
    }
}
