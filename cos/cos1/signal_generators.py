import math
from collections import namedtuple
from enum import Enum
from array import array

HarmonicParameters = namedtuple('HarmonicParameters', ['amplitude', 'frequency', 'initial_phase'])


class HarmonicSignalGenerator:
    def __init__(self, harmonic_params):
        self.__params = harmonic_params

    def get_signal_part(self, n, period):
        return self.__params.amplitude * math.sin(2 * math.pi * self.__params.frequency * n / period
                                                  + self.__params.initial_phase)

    def get_signal(self, period):
        for n in range(period):
            yield self.get_signal_part(n, period)


class PolyHarmonicSignalGenerator:
    def __init__(self, harmonic_params_collection):
        self.__harmonic_generators = []
        for harmonic_params in harmonic_params_collection:
            self.__harmonic_generators.append(HarmonicSignalGenerator(harmonic_params))

    def get_signal_part(self, n, period):
        return sum([generator.get_signal_part(n, period) for generator in self.__harmonic_generators])

    def get_signal(self, period):
        for n in range(period):
            yield self.get_signal_part(n, period)


class MutationType(Enum):
    INCREMENT = 1
    DECREMENT = -1


HarmonicMutations = namedtuple('HarmonicMutations',
                               ['amplitude_mutation', 'frequency_mutation', 'initial_phase_mutation'])


class LinearPolyHarmonicSignalGenerator:
    def __init__(self, harmonic_params_collection):
        self.__harmonic_params_collection = harmonic_params_collection

    def get_signal(self, period, period_iterations, mutation_per_period, mutation_type):
        mutation_per_signal_part = mutation_per_period / period
        mutation_multiplier = mutation_type.value
        mutations = []
        for harmonic_params in self.__harmonic_params_collection:
            mutations.append(
                HarmonicMutations(harmonic_params.amplitude * mutation_per_signal_part * mutation_multiplier,
                                  harmonic_params.frequency * mutation_per_signal_part * mutation_multiplier,
                                  harmonic_params.initial_phase * mutation_per_signal_part * mutation_multiplier))
        for n in range(period * period_iterations):
            new_harmonic_params_collection = []
            for index, harmonic_params in enumerate(self.__harmonic_params_collection):
                new_harmonic_params_collection.append(
                    HarmonicParameters(harmonic_params.amplitude + mutations[index].amplitude_mutation,
                                       harmonic_params.frequency + mutations[index].frequency_mutation,
                                       harmonic_params.initial_phase + mutations[index].initial_phase_mutation))
            self.__harmonic_params_collection = new_harmonic_params_collection
            yield PolyHarmonicSignalGenerator(self.__harmonic_params_collection).get_signal_part(n, period)


class HarmonicSignalGenerator2:
    def __init__(self, initial_phase, k_generator):
        self.__initial_phase = initial_phase
        self.__k_generator = k_generator

    def __get_signal_part(self, n, period):
        return math.sin(2 * math.pi * n / period + self.__initial_phase)

    def __get_signal(self, m, period):
        for n in range(m):
            yield self.__get_signal_part(n, period)

    def get_signals(self, period):
        m_step = math.floor(period / 4 / 8)
        m_collection = array('i')
        k = self.__k_generator(period)
        for cur_m in range(period, k, -m_step):
            m_collection.append(cur_m)
            cur_m -= m_step
        m_collection.reverse()
        for cur_m in range(period + m_step, 2 * period, m_step):
            m_collection.append(cur_m)

        signals = []
        for m in m_collection:
            signals.append(self.__get_signal(m, period))

        return signals


class HarmonicSignalAnalyzer:
    def __init__(self, harmonic_signal):
        self.__signal = harmonic_signal

    def __get_sum(self):
        return sum(self.__signal)

    def __get_sum_of_squared(self):
        return sum([signal_part ** 2 for signal_part in self.__signal])

    def get_root_mean_square_value(self):
        return math.sqrt(self.__get_sum_of_squared() / len(self.__signal))

    def get_standard_deviation(self):
        signals_count = len(self.__signal)
        return math.sqrt(self.__get_sum_of_squared() / signals_count - (self.__get_sum() / signals_count) ** 2)

    def get_amplitude(self):
        max_amplitude = 0
        signal_length = len(self.__signal)
        for k in range(math.floor(signal_length / 2)):
            complex_amplitude = complex()
            for n, x in enumerate(self.__signal):
                trigonometric_arg = 2 * math.pi * k * n / signal_length
                complex_amplitude += x * (math.cos(trigonometric_arg) + 1j * math.sin(trigonometric_arg))
            max_amplitude = max(max_amplitude, 2 * math.sqrt(complex_amplitude.real ** 2 + complex_amplitude.imag ** 2)
                                / signal_length)
        return max_amplitude

    def get_root_mean_square_value_error(self):
        return 0.707 - self.get_root_mean_square_value()

    def get_amplitude_error(self):
        return 1 - self.get_amplitude()
