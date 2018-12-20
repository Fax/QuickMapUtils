using SharpNoise;
using SharpNoise.Builders;
using SharpNoise.Modules;
using System;

namespace QuickMap
{
    static class AppConfig
    {
        public static int ZoomFactor = 1;

        public const int WORLD_WIDTH = 200;
        public const int WORLD_HEIGHT = 60;
        public const int SCREEN_WIDTH = 200;
        public const int SCREEN_HEIGHT = 80;
        public const int CIVILIZED_CIVS = 2;
        public const int TRIBAL_CIVS = 2;
        public const int MIN_RIVER_LENGHT = 3;
        public const int CIV_MAX_SITES = 20;
        public const int EXPANSION_DISTANCE = 10;
        public const int WAR_DISTANCE = 8;
    }

    class Program
    {

        static int GetSeed(string[] args)
        {
            var seed = new Random().Next();
            if (args.Length == 0) return seed;
            Int32.TryParse(args[0], out seed);
            return seed;
        }
        static void Main(string[] args)
        {
            bool reseed = true;
            int seed = 0;

            while (true)
            {
                if (reseed || seed == 0)
                    seed = GetSeed(args);
                reseed = true;
                var gen = new Generator(seed);
                gen.Configuration();
                var map = gen.MasterWorldGen();
                gen.PrintMap(map);
                var res = Console.ReadKey();
                if (res.KeyChar.Equals('q'))
                {
                    Environment.Exit(0);
                }

                if (res.KeyChar.Equals('+'))
                {
                    AppConfig.ZoomFactor++;
                    reseed = false;
                }
                if (res.KeyChar.Equals('-'))
                {
                    AppConfig.ZoomFactor--;
                    reseed = false;
                }

                if (AppConfig.ZoomFactor < 1)
                {
                    AppConfig.ZoomFactor = 1;
                }

            }
        }

    }

    public class Generator
    {
        Random random = new Random();
        public Generator(int seed)
        {
            // this static property needs to be reinitialized on each run. 
            random = new Random(seed);
        }
        public delegate void Logger(string message);
        private Logger _l;
        public Logger L
        {
            get
            {
                if (_l == null)
                {
                    _l = (x) => { Console.WriteLine(x); };
                }
                return _l;
            }
            set => _l = value;
        }

        public void Configuration()
        {
            L($"WORLD_WIDTH:{AppConfig.WORLD_WIDTH}");
            L($"WORLD_HEIGHT:{AppConfig.WORLD_HEIGHT}");
            L($"SCREEN_WIDTH:{AppConfig.SCREEN_WIDTH}");
            L($"CIVILIZED_CIVS:{AppConfig.CIVILIZED_CIVS}");
            L($"TRIBAL_CIVS:{AppConfig.TRIBAL_CIVS}");
            L($"MIN_RIVER_LENGHT:{AppConfig.MIN_RIVER_LENGHT}");
            L($"CIV_MAX_SITES:{AppConfig.CIV_MAX_SITES}");
            L($"EXPANSION_DISTANCE:{AppConfig.EXPANSION_DISTANCE}");
            L($"WAR_DISTANCE:{AppConfig.WAR_DISTANCE}");
        }


        public float[] MasterWorldGen()
        {
            Console.Clear();

            var noiseSource = new Perlin
            {
                Seed = random.Next(0, 100000)
            };
            var noiseMap = new NoiseMap();
            var noiseMapBuilder = new PlaneNoiseMapBuilder
            {
                DestNoiseMap = noiseMap,
                SourceModule = noiseSource
            };

            noiseMapBuilder.SetDestSize(AppConfig.WORLD_WIDTH, AppConfig.WORLD_HEIGHT);
            noiseMapBuilder.SetBounds(-1 * AppConfig.ZoomFactor, 1 * AppConfig.ZoomFactor, -1 * AppConfig.ZoomFactor, 1 * AppConfig.ZoomFactor);
            noiseMapBuilder.Build();

            return noiseMap.Data;

        }

        private void PrintTile(float cell)
        {

            if (cell < 0) { Console.Write("."); return; }
            if (cell < .1) { Console.Write("0"); return; }
            if (cell < .2) { Console.Write("1"); return; }
            if (cell < .3) { Console.Write("2"); return; }
            if (cell < .4) { Console.Write("3"); return; }
            if (cell < .5) { Console.Write("4"); return; }
            if (cell < .6) { Console.Write("5"); return; }
            if (cell < .7) { Console.Write("6"); return; }
            if (cell < .8) { Console.Write("7"); return; }
            if (cell < 1) { Console.Write("8"); }
        }

        public void PrintMap(float[] m)
        {
            for (var j = 0; j < AppConfig.WORLD_HEIGHT; j++)
            {
                for (var i = 0; i < AppConfig.WORLD_WIDTH; i++)
                {
                    var pos = j * AppConfig.WORLD_WIDTH + i;
                    var cell = m[pos];
                    PrintTile(cell);
                }
                Console.WriteLine("");
            }
        }



    }





}

