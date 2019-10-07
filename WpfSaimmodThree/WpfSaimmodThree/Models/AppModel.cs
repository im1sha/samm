using System;

namespace WpfSaimmodThree.Models
{
    internal class AppModel
    {
        private readonly double _busyProbability1;
        private readonly double _busyProbability2;

        public AppModel(double probability1, double probability2)
        {
            _busyProbability1 = probability1;
            _busyProbability2 = probability2;
        }

        private readonly Random _random1 = new Random();
        private readonly Random _random2 = new Random(42);

        private bool ChannelIsBusy1 => _random1.NextDouble() <= _busyProbability1;
        private bool ChannelIsBusy2 => _random2.NextDouble() <= _busyProbability2;


    }
}
