namespace Renderer
{
    public struct TextureData
    {
        public string name;
        public Texture texture;

        public TextureData(string name, Texture texture)
        {
            this.name = name;
            this.texture = texture;
        }
    }
}