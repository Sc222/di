﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mime;
using TagsCloudTextPreparation;
using TagsCloudTextPreparation.Excluders;
using TagsCloudTextPreparation.Formatters;
using TagsCloudTextPreparation.Shufflers;
using TagsCloudTextPreparation.Splitters;
using TagsCloudTextPreparation.Tokenizers;
using TagsCloudVisualization;
using TagsCloudVisualization.Layouters;
using TagsCloudVisualization.Styling;
using TagsCloudVisualization.Styling.Themes;
using TagsCloudVisualization.Styling.WordSizeCalculators;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var wordsToExclude = File.ReadAllLines(Path.Combine(projectDirectory, "Example", "wordsToExclude.txt"));
            var textInput = File.ReadAllText(Path.Combine(projectDirectory, "Example", "textInput.txt"));
            
            var wordsInput = new TextSplitter().SplitText(textInput);
            var wordsAfterFormat = new WordsFormatterLowercaseAndTrim().Format(wordsInput);
            var wordsAfterExclusion = new WordsExcluder().ExcludeWords(wordsAfterFormat, wordsToExclude);
            var tokens = new Tokenizer().Tokenize(wordsAfterExclusion);
            var shuffledTokens = new TokenShufflerRandom(10).Shuffle(tokens);
            
            var fontProperties = new FontProperties("Arial Black", 60);
            var style = new Style(new GrayTheme(), fontProperties, new TagSizeCalculatorLogarithmic());
            var visualizer = new TextNoRectanglesVisualizer();
            var layouter = new SpiralCloudLayouter(new Spiral(new PointF(600, 700), 0.1f, 0.1f));
            
            var cloud = new Cloud(shuffledTokens, style, visualizer, layouter);

            cloud
                .Visualize(1500, 1500)
                .Save(Path.Combine(projectDirectory, "Example", "result.png"), ImageFormat.Png);
        }
    }
}