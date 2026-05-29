using BH.SDK.Models;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Values;
using BH.SDK.Utils;

namespace BH.SDK.Generators
{
    public class TextureObjectsGenerator : ILevelGenerator<TextureObjectsParameters>
    {
        public void Generate(Level level, TextureObjectsParameters parameters)
        {
            var widthMult = parameters.Width / (float)parameters.Texture.Width;
            var heightMult = parameters.Height / (float)parameters.Texture.Height;

            var parentObj = new Object();
            parentObj.SetObjectId(level.Settings);
            parentObj.SetBounds(parameters.StartFrame, parameters.EndFrame);
            level.Game.Objects.Add(parentObj);
            
            var indexer = new DimensionalIndexer2(parameters.Width, parameters.Height);
            using var enumerator = indexer.GetEnumerator2();
            while (enumerator.MoveNext())
            {
                var pixel = parameters.Texture.Pixels[indexer.GetIndex(enumerator.Current)];
                var mappedIndexX = (int)(enumerator.Current.IndexWidth * widthMult);
                var mappedIndexY = (int)(enumerator.Current.IndexHeight * heightMult);

                var textureObj = new TextureObject();
                textureObj.SetObjectId(level.Settings);
                textureObj.SetParent(parentObj);
                textureObj.SetBounds(parameters.StartFrame, parameters.EndFrame);

                var posVec = new Vector2Value(mappedIndexX, -mappedIndexY);
                var pos = new PosKey(posVec, parameters.StartFrame);
                var pivot = new AlignmentKey(parameters.PixelPivot.Value.Copy(), parameters.StartFrame);
                var sca = new ScaKey(parameters.PixelSca.Copy(), parameters.StartFrame);
                var clr = new ColorKey(pixel.ToColorValue(), parameters.StartFrame);
                var layer = new LayerKey(new IntValue(parameters.Layer), parameters.StartFrame);
                
                textureObj.Positions.Add(pos);
                textureObj.Layers.Add(layer);
                textureObj.Scales.Add(sca);
                textureObj.Pivots.Add(pivot);
                textureObj.Colors.Add(clr);
            }
        }
    }
}