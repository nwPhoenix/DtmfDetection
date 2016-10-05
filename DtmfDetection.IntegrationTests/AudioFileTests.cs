﻿namespace DtmfDetection.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NAudio;

    using global::NAudio.Wave;
    using global::NAudio.Vorbis;
    
    [TestClass]
    public class AudioFileTests
    {
        [TestMethod]
        public void DetectsLongDtmfTonesCorrectly()
        {
            #region Expected tones

            var expectedTones = new[]
            {
                new DtmfPosition(new DtmfOccurence(DtmfTone.One, 0), TimeSpan.FromSeconds(2.819), TimeSpan.FromSeconds(2.819)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Two, 0), TimeSpan.FromSeconds(6.766), TimeSpan.FromSeconds(0.153)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Three, 0), TimeSpan.FromSeconds(7.329), TimeSpan.FromSeconds(0.180)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Four, 0), TimeSpan.FromSeconds(8.252), TimeSpan.FromSeconds(2.357)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Five, 0), TimeSpan.FromSeconds(12.070), TimeSpan.FromSeconds(0.026)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Five, 0), TimeSpan.FromSeconds(12.121), TimeSpan.FromSeconds(0.205)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Six, 0), TimeSpan.FromSeconds(12.762), TimeSpan.FromSeconds(0.102)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Seven, 0), TimeSpan.FromSeconds(14.556), TimeSpan.FromSeconds(0.025)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Seven, 0), TimeSpan.FromSeconds(14.607), TimeSpan.FromSeconds(2.306)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Eight, 0), TimeSpan.FromSeconds(17.733), TimeSpan.FromSeconds(0.103)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Nine, 0), TimeSpan.FromSeconds(18.476), TimeSpan.FromSeconds(0.103)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Hash, 0), TimeSpan.FromSeconds(19.168), TimeSpan.FromSeconds(0.256)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Zero, 0), TimeSpan.FromSeconds(19.886), TimeSpan.FromSeconds(0.051)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.Star, 0), TimeSpan.FromSeconds(20.501), TimeSpan.FromSeconds(0.153)),
                new DtmfPosition(new DtmfOccurence(DtmfTone.One, 0), TimeSpan.FromSeconds(22.064), TimeSpan.FromSeconds(1.768))
            };

            #endregion

            using (var waveFile = new Mp3FileReader("TestData/long_dtmf_tones.mp3"))
            {
                var actualTones = waveFile.DtmfTones().ToArray();

                AssertEqual(expectedTones, actualTones);
            }
        }

        /// <summary>Test data has been taken from https://en.wikipedia.org/wiki/File:DTMF_dialing.ogg (no license, public domain). 
        /// Mark/space of the DTMF sequences is about 60/40.</summary>
        [TestMethod]
        public void DetectsVeryShortDtmfTonesCorrectly()
        {
            #region Expected tones

            var expectedTones = new[]
            {
                DtmfTone.Zero, DtmfTone.Six, DtmfTone.Nine, DtmfTone.Six, DtmfTone.Six, DtmfTone.Seven, DtmfTone.Five, DtmfTone.Three, DtmfTone.Five, DtmfTone.Six,

                DtmfTone.Four, DtmfTone.Six, DtmfTone.Four, DtmfTone.Six, DtmfTone.Four, DtmfTone.One, DtmfTone.Five, DtmfTone.One, DtmfTone.Eight, DtmfTone.Zero,

                DtmfTone.Two, DtmfTone.Three, DtmfTone.Three, DtmfTone.Six, DtmfTone.Seven, DtmfTone.Three, DtmfTone.One, DtmfTone.Four, DtmfTone.One, DtmfTone.Six,

                DtmfTone.Three, DtmfTone.Six, DtmfTone.Zero, DtmfTone.Eight, DtmfTone.Three, DtmfTone.Three, DtmfTone.Eight, DtmfTone.One, DtmfTone.Six, DtmfTone.Zero,

                DtmfTone.Four, DtmfTone.Four, DtmfTone.Zero, DtmfTone.Zero, DtmfTone.Eight, DtmfTone.Two, DtmfTone.Six, DtmfTone.One, DtmfTone.Four, DtmfTone.Six,

                DtmfTone.Six, DtmfTone.Two, DtmfTone.Five, DtmfTone.Three, DtmfTone.Six, DtmfTone.Eight, DtmfTone.Nine, DtmfTone.Six, DtmfTone.Three, DtmfTone.Eight,

                DtmfTone.Eight, DtmfTone.Four, DtmfTone.Eight, DtmfTone.Two, DtmfTone.One, DtmfTone.Three, DtmfTone.Eight, DtmfTone.One, DtmfTone.Seven, DtmfTone.Eight,

                DtmfTone.Five, DtmfTone.Zero, DtmfTone.Seven, DtmfTone.Three, DtmfTone.Six, DtmfTone.Four, DtmfTone.Three, DtmfTone.Three, DtmfTone.Nine, DtmfTone.Nine
            };

            #endregion

            using (var waveFile = new VorbisWaveReader("TestData/very_short_dtmf_tones.ogg"))
            {
                var actualTones = waveFile.DtmfTones().Select(t => t.DtmfTone).ToArray();

                AssertEqual(expectedTones, actualTones);
            }
        }

        private static void AssertEqual<T>(IList<T> expecteds, IList<T> actuals)
        {
            Assert.AreEqual(expecteds.Count, actuals.Count, "The lists are not of the same length.");

            for (var i = 0; i < Math.Min(expecteds.Count, actuals.Count); i++)
                Assert.AreEqual(expecteds[i], actuals[i], $"Items at index {i} do not match");
        }
    }
}
