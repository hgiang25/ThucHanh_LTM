using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab6
{
    public enum ActionType
    {
        Line,
        Image
    }

    public class DrawAction
    {
        public ActionType Type { get; set; }

        // Properties for line drawing
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        [JsonConverter(typeof(JsonColorConverter))]
        public Color PenColor { get; set; }
        public float PenWidth { get; set; }

        // Properties for image
        public string? ImageUrl { get; set; }
        public Rectangle? ImageRect { get; set; }
        [JsonIgnore]
        public Image? Image { get; set; } // Not serialized, loaded from ImageUrl

        public void Draw(Graphics g)
        {
            if (Type == ActionType.Line)
            {
                using (Pen pen = new Pen(PenColor, PenWidth))
                {
                    g.DrawLine(pen, StartPoint, EndPoint);
                }
            }
            else if (Type == ActionType.Image && Image != null && ImageRect.HasValue)
            {
                g.DrawImage(Image, ImageRect.Value);
            }
        }
    }

    public class JsonColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? colorString = reader.GetString();
            return colorString != null ? ColorTranslator.FromHtml(colorString) : Color.Black;
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(ColorTranslator.ToHtml(value));
        }
    }
}