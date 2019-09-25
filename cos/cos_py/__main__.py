import argparse
from chart_drawer import *
from signal_generators import *


def task_1():  # v5
    def harmonic():
        variable_initial_phase_parameters = []
        for initial_phase in [math.pi, 0.0, math.pi / 3, math.pi / 6, math.pi / 2]:
            variable_initial_phase_parameters.append(HarmonicParameters(7, 5, initial_phase))

        variable_frequency_parameters = []
        for frequency in [1, 5, 11, 6, 3]:
            variable_frequency_parameters.append(HarmonicParameters(5, frequency, 3 * math.pi / 4))

        variable_amplitude_parameters = []
        for amplitude in [1, 2, 11, 4, 2]:
            variable_amplitude_parameters.append(HarmonicParameters(amplitude, 3, 3 * math.pi / 4))

        variable_parameters_choice = {'initial-phase': variable_initial_phase_parameters,
                                      'frequency': variable_frequency_parameters,
                                      'amplitude': variable_amplitude_parameters}
        parser = argparse.ArgumentParser()
        parser.add_argument('-v', '--variable-parameter',
                            action='store',
                            required=True,
                            help='what parameter should vary',
                            choices=variable_parameters_choice.keys(),
                            dest='parameter',
                            type=str)
        parser.add_argument('-N', '--period',
                            action='store',
                            required=False,
                            help='signal period',
                            dest='period',
                            type=int,
                            default=512)
        args = parser.parse_known_args()[0]

        chart_data = []
        for harmonic_params in variable_parameters_choice[args.parameter]:
            signal = list(HarmonicSignalGenerator(harmonic_params).generate_signal(args.period))
            chart_data.append(LabeledChartData(
                range(len(signal)),
                signal,
                ', '.join(['A: ' + str(harmonic_params.amplitude),
                           'f: ' + str(harmonic_params.frequency),
                           'phi: ' + str(harmonic_params.initial_phase)])))
        draw_charts(chart_data)

    def polyharmonic():
        parser = argparse.ArgumentParser()
        parser.add_argument('-N', '--period', action='store', required=False, help='signal period', dest='period',
                            type=int, default=512)
        period = parser.parse_known_args()[0].period
        harmonic_parameters = [HarmonicParameters(1, 1, 0), HarmonicParameters(1, 2, math.pi / 4),
                               HarmonicParameters(1, 3, math.pi / 6), HarmonicParameters(1, 4, 2 * math.pi),
                               HarmonicParameters(1, 5, math.pi)]
        signal = list(PolyharmonicSignalGenerator(harmonic_parameters).generate_signal(period))
        draw_chart(LabeledChartData(range(len(signal)), signal, None))

    def linear():
        parser = argparse.ArgumentParser()
        parser.add_argument('-N', '--period', action='store', required=False, help='signal period', dest='period',
                            type=int, default=512)
        parser.add_argument('-i', '--period-iterations', action='store', required=False, help='period iterations',
                            dest='period_iterations', type=int, default=1)
        parser.add_argument('-m', '--mutation', action='store', required=False, help='mutation per period',
                            dest='mutation_per_period', type=float, default=0.2)
        parser.add_argument('-ml', '--mutation-law', action='store', required=True, help='mutation law', type=str,
                            dest='mutation_law', choices=[MutationType.DECREMENT.name, MutationType.INCREMENT.name])
        args = parser.parse_known_args()[0]
        harmonic_parameters = [HarmonicParameters(1, 1, 0), HarmonicParameters(1, 2, math.pi / 4),
                               HarmonicParameters(1, 3, math.pi / 6), HarmonicParameters(1, 4, 2 * math.pi),
                               HarmonicParameters(1, 5, math.pi)]
        signal = list(LinearPolyharmonicSignalGenerator(harmonic_parameters)
                      .generate_signal(args.period,
                                       args.period_iterations,
                                       args.mutation_per_period,
                                       MutationType[args.mutation_law]))

        draw_chart(LabeledChartData(range(len(signal)), signal, None))

    sub_tasks_callbacks = {'harmonic': harmonic, 'polyharmonic': polyharmonic, 'linear': linear}

    parser = argparse.ArgumentParser()
    parser.add_argument('-st', '--sub-task', action='store', required=True, help='first tasks sub task',
                        choices=sub_tasks_callbacks.keys(), dest='sub_task', type=str)
    sub_tasks_callbacks[parser.parse_known_args()[0].sub_task]()


def task_2():
    parser = argparse.ArgumentParser()
    parser.add_argument('-p', '--with-phase', action='store_true', help='should phase be used', dest='with_phase')
    parser.add_argument('-N', '--period', action='store', required=False, help='signal period', dest='period',
                        type=int, default=512)
    args = parser.parse_known_args()[0]
    initial_phase = math.pi / 32 if args.with_phase else 0
    k_generator = lambda period: math.ceil(3 * period / 4)

    amplitude_error_chart_data = {}
    root_mean_square_error_chart_data = {}

    for signal in HarmonicSignalGenerator2(initial_phase, k_generator).get_signals(args.period):
        listed_signal = list(signal)
        signal_length = len(listed_signal)

        analyzer = HarmonicSignalAnalyzer(listed_signal)
        print("Сигнал с M =", signal_length, ":")
        print("\tсреднее квадратическое значение:", analyzer.get_root_mean_square_value())
        print("\tсреднее квадратическое отклонение:", analyzer.get_standard_deviation())
        print("\tамплитуда:", analyzer.get_amplitude())
        root_mean_square_error = analyzer.get_root_mean_square_value_error()
        root_mean_square_error_chart_data[signal_length] = root_mean_square_error
        print("\tпогрешность СКЗ:", root_mean_square_error)
        amplitude_error = analyzer.get_amplitude_error()
        amplitude_error_chart_data[signal_length] = amplitude_error
        print("\tпогрешность амплитуды:", amplitude_error)

    draw_charts([
        LabeledChartData(amplitude_error_chart_data.keys(), amplitude_error_chart_data.values(), "Amplitude error"),
        LabeledChartData(root_mean_square_error_chart_data.keys(), root_mean_square_error_chart_data.values(),
                         "Root mean square error")
    ])


def main():
    tasks_callbacks = {1: task_1, 2: task_2}

    parser = argparse.ArgumentParser()
    parser.add_argument('-t', '--task',
                        action='store',
                        required=True,
                        help='task number',
                        choices=tasks_callbacks.keys(),
                        dest='task',
                        type=int)
    tasks_callbacks[parser.parse_known_args()[0].task]()


if __name__ == "__main__":
    main()
