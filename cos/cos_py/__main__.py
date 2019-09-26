import argparse
from chart_drawer import *
from signal_generators import *
from local_parser import LocalParser


# harmonic| impulse| triangle| saw_edged| noise functions handles exact Nargs signals
# and returns [[signal1], [signal2], [signalNargs]]
# DEFAULTS: store=True, only_last=False, exact_at=-1

def harmonic(nargs, store_last_group=True, only_last=False, exact_at=-1):
    parser = LocalParser(argparse.ArgumentParser())

    period = parser.get_length(nargs, store_last_group, only_last, exact_at)
    amplitudes = parser.get_amplitudes(nargs, store_last_group, only_last, exact_at)  # []

    frequencies = [1]
    __repeat(frequencies, nargs)
    # parser.get_frequencies(nargs, store_last_group, only_last, exact_at)  # []

    initial_phases = parser.get_initial_phases(nargs, store_last_group, only_last, exact_at)  # []

    signals = list(list(HarmonicSignalGenerator(p, HarmonicParameters(a, f, i))
                        .generate_signal()) for p, a, f, i in zip(period, amplitudes, frequencies, initial_phases))
    return signals


def impulse(nargs, store_last_group=True, only_last=False, exact_at=-1):
    parser = LocalParser(argparse.ArgumentParser())

    length = parser.get_length(nargs, store_last_group, only_last, exact_at)
    amplitudes = parser.get_amplitudes(nargs, store_last_group, only_last, exact_at)

    duty_circles = parser.get_duty_circles(nargs, store_last_group, only_last, exact_at)

    signals = list(list(ImpulseSignalGenerator(p, a, d)
                        .generate_signal()) for p, a, d in zip(length, amplitudes, duty_circles))
    return signals


def triangle(nargs, store_last_group=True, only_last=False, exact_at=-1):
    parser = LocalParser(argparse.ArgumentParser())

    amplitudes = parser.get_amplitudes(nargs, store_last_group, only_last, exact_at)
    period = parser.get_length(nargs, store_last_group, only_last, exact_at)

    signals = list(list(TriangleSignalGenerator(p, a).generate_signal()) for p, a in zip(period, amplitudes))
    return signals


def saw_edged(nargs, store_last_group=True, only_last=False, exact_at=-1):
    parser = LocalParser(argparse.ArgumentParser())
    amplitudes = parser.get_amplitudes(nargs, store_last_group, only_last, exact_at)
    length = parser.get_length(nargs, store_last_group, only_last, exact_at)
    growings = parser.get_growings(nargs, store_last_group, only_last, exact_at)
    signals = list(list(SawEdgedSignalGenerator(p, a, g).generate_signal())
                   for p, a, g in zip(length, amplitudes, growings))
    return signals


def noise(nargs, store_last_group=True, only_last=False, exact_at=-1):
    parser = LocalParser(argparse.ArgumentParser())
    amplitudes = parser.get_amplitudes(nargs, store_last_group, only_last, exact_at)
    length = parser.get_length(nargs, store_last_group, only_last, exact_at)
    signals = list(list(NoiseSignalGenerator(p, a).generate_signal()) for p, a in zip(length, amplitudes))
    return signals


# only calls appropriate signal creation function

def poly(task, nargs):
    arrays = task(nargs)
    return [list(PolyharmonicSignalGenerator(len(arrays[0]), arrays).generate_signal())]


def f_modulate(tasks):
    print('fm')


# m_task values - [information signal]
# c_task values - [carrier signal]

def a_modulate(m_task, c_task):
    m_values = m_task(1, False, False)[0]
    c_values = c_task(1, False, True)[0]
    if len(m_values) != len(c_values):
        raise Exception()
    return [list(AmplitudeModulator(len(m_values), m_values, c_values).generate_signal()), m_values, c_values]


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


def __repeat(arr, times):
    res = []
    for _ in range(times):
        res += arr


def main():
    signals_callbacks = {
        'harmonic': harmonic,
        'impulse': impulse,
        'triangle': triangle,
        'sawedged': saw_edged,
        'noise': noise
    }
    modulate_callbacks = {
        'fm': f_modulate,
        'am': a_modulate,
        'none': None
    }

    parser = LocalParser(argparse.ArgumentParser())
    modulation = parser.get_modulation(modulate_callbacks)

    # if (modulation != None) then values from CLI stores
    # as appended([[value1], [value2], [value3]])
    # else
    # as stored([value1, value2, valueNargs])
    if modulation is None:
        nargs = parser.get_nargs()
        tasks = parser.get_tasks(signals_callbacks)
    else:
        nargs = 1
        tasks = parser.get_tasks(signals_callbacks, False)
        if len(tasks) != 2:
            raise Exception('It should be 2 signals for modeling')

    # expected that one_signal_arrays is  [[]] == [[chart1 values], [chart2 values] ...]
    if modulation is not None:
        signal = modulation(tasks[0], tasks[1])  # tasks == [m_task, c_task]
    elif not parser.get_is_polyharmonic():
        signal = tasks[0](nargs)
    else:
        signal = poly(tasks[0], nargs)

    chart_data = []
    for arr in signal:
        chart_data += [LabeledChartData(
            range(len(arr)),
            arr,
            None)]

    draw_charts(chart_data)


if __name__ == "__main__":
    main()
