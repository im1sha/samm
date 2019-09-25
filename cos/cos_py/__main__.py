import argparse
from chart_drawer import *
from signal_generators import *


class LocalParser:
    def __init__(self, parser):
        self.__parser = parser
        self.__args = []
        self.__tasks_callbacks = {}

    def get_length(self) -> int:
        self.__parser.add_argument('-N', '--length',
                                   action='store',
                                   required=False,
                                   help='signal length',
                                   dest='length',
                                   type=int,
                                   default=512)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.length

    def get_amplitude(self) -> float:
        self.__parser.add_argument('-A', '--amplitude',
                                   action='store',
                                   required=False,
                                   help='signal amplitude',
                                   dest='amplitude',
                                   type=float,
                                   default=10)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.amplitude

    def get_task(self, tasks_callbacks):
        self.__tasks_callbacks = tasks_callbacks
        self.__parser.add_argument('-t', '--task',
                                   action='store',
                                   required=True,
                                   help='task name',
                                   choices=tasks_callbacks.keys(),
                                   dest='task',
                                   type=str)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__tasks_callbacks[self.__args.task]

    def get_frequency(self) -> float:
        self.__parser.add_argument('-f', '--frequency',
                                   action='store',
                                   required=False,
                                   help='frequency',
                                   dest='frequency',
                                   type=float,
                                   default=1.0)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.frequency

    def get_initial_phase(self) -> float:
        self.__parser.add_argument('-p', '--initial-phase',
                                   action='store',
                                   required=False,
                                   help='initial phase',
                                   dest='initial_phase',
                                   type=float,
                                   default=0.0)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.initial_phase

    def get_iterations(self) -> int:
        self.__parser.add_argument('-i', '--iterations',
                                   action='store',
                                   required=False,
                                   help='total iterations',
                                   dest='iterations',
                                   type=int,
                                   default=5)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.iterations

    def get_duty_circle(self) -> float:
        self.__parser.add_argument('-D', '--duty-circle',
                                   action='store',
                                   required=False,
                                   help='signal length percentage',
                                   dest='duty_circle',
                                   type=float,
                                   default=0.25)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.duty_circle

    def get_growing(self) -> bool:
        self.__parser.add_argument('-g', '--growing',
                                   action='store',
                                   required=False,
                                   help='chart direction',
                                   dest='growing',
                                   type=str,
                                   default='yes')
        self.__args = self.__parser.parse_known_args()[0]
        return (self.__args.growing == 'y') or (self.__args.growing == 'yes')


def harmonic():
    parser = LocalParser(argparse.ArgumentParser())
    period = parser.get_length()
    amplitude = parser.get_amplitude()
    frequency = parser.get_frequency()
    initial_phase = parser.get_initial_phase()

    signal = list(HarmonicSignalGenerator(period, HarmonicParameters(amplitude, frequency, initial_phase))
                  .generate_signal())
    return signal


def impulse():
    parser = LocalParser(argparse.ArgumentParser())
    length = parser.get_length()
    amplitude = parser.get_amplitude()
    duty_circle = parser.get_duty_circle()

    signal = list(ImpulseSignalGenerator(length, amplitude, duty_circle)
                  .generate_signal())
    return signal


def triangle():
    parser = LocalParser(argparse.ArgumentParser())
    amplitude = parser.get_amplitude()
    period = parser.get_length()

    signal = list(TriangleSignalGenerator(period, amplitude).generate_signal())
    return signal


def saw_edged():
    parser = LocalParser(argparse.ArgumentParser())
    amplitude = parser.get_amplitude()
    length = parser.get_length()
    growing = parser.get_growing()
    return list(SawEdgedSignalGenerator(length, amplitude, growing).generate_signal())


def noise():
    parser = LocalParser(argparse.ArgumentParser())
    amplitude = parser.get_amplitude()
    length = parser.get_length()
    signal = list(NoiseSignalGenerator(length, amplitude).generate_signal())
    return signal


# def task_1b():
#
#     # todo add ability of signal generation based on 1a
#     parser = argparse.ArgumentParser()
#     parser.add_argument('-N', '--period',
#                         action='store',
#                         required=False,
#                         help='signal period',
#                         dest='period',
#                         type=int,
#                         default=512)
#     period = parser.parse_known_args()[0].period
#     harmonic_parameters = [HarmonicParameters(9, 1, math.pi / 2),
#                            HarmonicParameters(9, 2, 0.0),
#                            HarmonicParameters(9, 3, math.pi / 4),
#                            HarmonicParameters(9, 4, math.pi / 3),
#                            HarmonicParameters(9, 5, math.pi / 6)]
#
#     signal = list(PolyharmonicSignalGenerator(harmonic_parameters).generate_signal(period))
#
#     draw_chart(LabeledChartData(range(len(signal)), signal, None))


# def task_1c():
#
#     # todo add ability of signal modulation based on 1a
#
#     parser = argparse.ArgumentParser()
#     parser.add_argument('-N', '--period',
#                         action='store',
#                         required=False,
#                         help='signal period',
#                         dest='period',
#                         type=int,
#                         default=512)
#     parser.add_argument('-i', '--period-iterations',
#                         action='store',
#                         required=False,
#                         help='period iterations',
#                         dest='period_iterations',
#                         type=int,
#                         default=1)
#     parser.add_argument('-m', '--mutation',
#                         action='store',
#                         required=False,
#                         help='mutation per period',
#                         dest='mutation_per_period',
#                         type=float,
#                         default=0.2)
#     parser.add_argument('-ml', '--mutation-law',
#                         action='store',
#                         required=True,
#                         help='mutation law',
#                         type=str,
#                         dest='mutation_law',
#                         choices=[MutationType.DECREMENT.name,
#                                  MutationType.INCREMENT.name])
#     args = parser.parse_known_args()[0]
#
#     harmonic_parameters = [HarmonicParameters(9, 1, math.pi / 2),
#                            HarmonicParameters(9, 2, 0.0),
#                            HarmonicParameters(9, 3, math.pi / 4),
#                            HarmonicParameters(9, 4, math.pi / 3),
#                            HarmonicParameters(9, 5, math.pi / 6)]
#
#     signal = list(LinearPolyharmonicSignalGenerator(harmonic_parameters)
#                   .generate_signal(args.period,
#                                    args.period_iterations,
#                                    args.mutation_per_period,
#                                    MutationType[args.mutation_law]))
#
#     draw_chart(LabeledChartData(range(len(signal)), signal, None))


# def task_2():
#     parser = argparse.ArgumentParser()
#     parser.add_argument('-p', '--with-phase', action='store_true', help='should phase be used', dest='with_phase')
#     parser.add_argument('-N', '--period', action='store', required=False, help='signal period', dest='period',
#                         type=int, default=512)
#     args = parser.parse_known_args()[0]
#     initial_phase = math.pi / 32 if args.with_phase else 0
#     k_generator = lambda period: math.ceil(3 * period / 4)
#
#     amplitude_error_chart_data = {}
#     root_mean_square_error_chart_data = {}
#
#     for signal in HarmonicSignalGenerator2(initial_phase, k_generator).get_signals(args.period):
#         listed_signal = list(signal)
#         signal_length = len(listed_signal)
#
#         analyzer = HarmonicSignalAnalyzer(listed_signal)
#         print("Сигнал с M =", signal_length, ":")
#         print("\tсреднее квадратическое значение:", analyzer.get_root_mean_square_value())
#         print("\tсреднее квадратическое отклонение:", analyzer.get_standard_deviation())
#         print("\tамплитуда:", analyzer.get_amplitude())
#         root_mean_square_error = analyzer.get_root_mean_square_value_error()
#         root_mean_square_error_chart_data[signal_length] = root_mean_square_error
#         print("\tпогрешность СКЗ:", root_mean_square_error)
#         amplitude_error = analyzer.get_amplitude_error()
#         amplitude_error_chart_data[signal_length] = amplitude_error
#         print("\tпогрешность амплитуды:", amplitude_error)
#
#     draw_charts([
#         LabeledChartData(amplitude_error_chart_data.keys(), amplitude_error_chart_data.values(), "Amplitude error"),
#         LabeledChartData(root_mean_square_error_chart_data.keys(), root_mean_square_error_chart_data.values(),
#                          "Root mean square error")
#     ])


def main():
    sub_tasks_callbacks = {'harmonic': harmonic,
                           'impulse': impulse,
                           'triangle': triangle,
                           'sawedged': saw_edged,
                           'noise': noise}

    parser = LocalParser(argparse.ArgumentParser())
    iterations = parser.get_iterations()

    one_signal = parser.get_task(sub_tasks_callbacks)()

    signal = []
    for _ in range(iterations):
        signal += one_signal

    chart_data = [LabeledChartData(
        range(len(signal)),
        signal,
        None)]
    draw_charts(chart_data)


if __name__ == "__main__":
    main()
