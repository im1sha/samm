import argparse
from chart_drawer import *
from signal_generators import *
from local_parser import LocalParser


def harmonic(nargs):
    parser = LocalParser(argparse.ArgumentParser())
    period = parser.get_length()

    # lists
    amplitudes = parser.get_amplitudes(nargs)  # []
    frequencies = parser.get_frequencies(nargs)  # []
    initial_phases = parser.get_initial_phases(nargs)  # []

    signals = list(list(HarmonicSignalGenerator(period, HarmonicParameters(a, f, i))
                        .generate_signal()) for a, f, i in zip(amplitudes, frequencies, initial_phases))
    return signals


def impulse(nargs):
    parser = LocalParser(argparse.ArgumentParser())
    length = parser.get_length()
    amplitudes = parser.get_amplitudes(nargs)
    duty_circles = parser.get_duty_circles(nargs)

    signals = list(list(ImpulseSignalGenerator(length, a, d)
                        .generate_signal()) for a, d in zip(amplitudes, duty_circles))
    return signals


def triangle(nargs):
    parser = LocalParser(argparse.ArgumentParser())
    amplitudes = parser.get_amplitudes(nargs)
    period = parser.get_length()

    signals = list(list(TriangleSignalGenerator(period, a).generate_signal()) for a in amplitudes)
    return signals


def saw_edged(nargs):
    parser = LocalParser(argparse.ArgumentParser())
    amplitudes = parser.get_amplitudes(nargs)
    length = parser.get_length()
    growings = parser.get_growings(nargs)
    signals = list(list(SawEdgedSignalGenerator(length, a, g).generate_signal()) for a, g in zip(amplitudes, growings))
    return signals


def noise(nargs):
    parser = LocalParser(argparse.ArgumentParser())
    amplitudes = parser.get_amplitudes(nargs)
    length = parser.get_length()
    signals = list(list(NoiseSignalGenerator(length, a).generate_signal()) for a in amplitudes)
    return signals


def poly(task, nargs):
    arrays = task(nargs)
    return [PolyharmonicSignalGenerator(len(arrays[0]), arrays).generate_signal()]


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
                           'noise': noise
                           }

    parser = LocalParser(argparse.ArgumentParser())
    iterations = parser.get_iterations()
    nargs = parser.get_nargs()
    task = parser.get_task(sub_tasks_callbacks)

    # expected that one_signal_arrays is  [[]] == [[chart1 values], [chart2 values] ...]
    if not parser.get_is_polyharmonic():
        one_signal_arrays = task(nargs)
    else:
        one_signal_arrays = poly(task, nargs)

    chart_data = []
    for arr in one_signal_arrays:
        to_append = []
        for _ in range(iterations):
            to_append += arr
        chart_data += [LabeledChartData(
            range(len(to_append)),
            to_append,
            None)]

    draw_charts(chart_data)


if __name__ == "__main__":
    main()
