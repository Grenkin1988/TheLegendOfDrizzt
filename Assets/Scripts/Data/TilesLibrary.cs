using System.IO;
using TheLegendOfDrizzt.Assets.Scripts.Utility;
using UnityEngine;

namespace TheLegendOfDrizzt.Assets.Scripts.Data {
    public static class TilesLibrary {
        public static readonly TileData StartTileLeft = new TileData {
            Name = "StartTileLeft",
            Layout = "Wall;Floor;Floor;Mashrooms;Floor;Floor;Floor;Floor;Floor;Floor;Floor;Floor;Wall;Wall;Wall;Wall"
        };

        public static readonly TileData StartTileRight = new TileData {
            Name = "StartTileRight",
            Layout = "Wall;Floor;Floor;Floor;Floor;Floor;Mashrooms;Floor;Floor;Floor;Floor;Floor;Wall;Wall;Wall;Wall"
        };

        public static TileData[] DefaultTiles;

        static TilesLibrary() {
            DefaultTiles = new TileData[] { };

            // Path.Combine combines strings into a file path
            // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
            string filePath = Path.Combine(Application.streamingAssetsPath, "Data/DefaultTiles.json");

            if (File.Exists(filePath)) {
                string dataAsJson = File.ReadAllText(filePath);
                TileData[] loadedData = JsonHelper.FromJson<TileData>(dataAsJson);

                DefaultTiles = loadedData;
            } else {
                Debug.LogError("Cannot load game data!");
            }
        }
    }
}
