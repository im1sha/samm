import math
import random
from collections import namedtuple
from enum import Enum
from array import array

HarmonicParameters = namedtuple('HarmonicParameters', ['amplitude', 'frequency', 'initial_phase'])


class SignalGenerator:
    def __init__(self, length, amplitude):
        self.__length = length
        self.__amplitude = amplitude

    def get_discrete_signal(self, n):
        return 0.0

    def generate_signal(self):
        for n in range(self.__length):  # 0..period-1
            yield self.get_discrete_signal(n)

    def length(self):
        return self.__length

    def amplitude(self):
        return self.__amplitude


class NoiseSignalGenerator(SignalGenerator):
    def __init__(self, length, amplitude):
        super().__init__(length, amplitude)

    def get_discrete_signal(self, n):
        return random.uniform(0, self.amplitude())


class TriangleSignalGenerator(SignalGenerator):
    def __init__(self, period, amplitude):
        super().__init__(period, amplitude)

    def get_discrete_signal(self, n):
        if n > self.length() / 2:
            return (1 - math.fabs(n / self.length())) * self.amplitude() * 2
        else:
            return (n / self.length()) * self.amplitude() * 2


class SawEdgedSignalGenerator(SignalGenerator):
    def __init__(self, length, amplitude, growing):
        super().__init__(length, amplitude)
        self.__growing = growing

    def get_discrete_signal(self, n):
        if self.__growing == 1:  # 0 == not growing | 1 == growing
            return (n / self.length()) * self.amplitude()
        else:
            return (1 - math.fabs(n / self.length())) * self.amplitude()


class ImpulseSignalGenerator(SignalGenerator):
    def __init__(self, length, amplitude, duty_circle):
        super().__init__(length, amplitude)
        self.__duty_circle = duty_circle

    def get_discrete_signal(self, n):
        if n < math.ceil(self.length() * self.__duty_circle):
            return self.amplitude()
        else:
            return 0


class HarmonicSignalGenerator(SignalGenerator):
    def __init__(self, period, harmonic_params):
        super().__init__(period, harmonic_params.amplitude)
        self.__frequency = harmonic_params.frequency
        self.__initial_phase = harmonic_params.initial_phase

    def get_discrete_signal(self, n):
        return (self.amplitude()
                * math.sin(2 * math.pi * self.__frequency * n / self.length()
                           + self.__initial_phase)) + self.amplitude()


class PolyharmonicSignalGenerator(SignalGenerator):
    def __init__(self, length, arrays):
        super().__init__(length, -1)  # amplitude is not in use
        self.__arrays = arrays  # [[]] == [[chart1 values], [chart2 values] ...]

    def get_discrete_signal(self, n):
        return sum(arr[n] for arr in self.__arrays)  # takes all values at n-th position


# m_array - [information signal]
# c_array - [carrier signal]

class AmplitudeModulator(SignalGenerator):
    def __init__(self, length, m_array, c_array, modulation_coefficient=1.0):
        super().__init__(length, -1)
        self.__m_array = m_array
        self.__c_array = c_array
        self.__modulation_coefficient = modulation_coefficient
        self.__m_max_abs = max(list(map(lambda x: math.fabs(x), self.__m_array)))

    def get_discrete_signal(self, n):
        return self.__c_array[n] * (1 + (self.__modulation_coefficient * self.__m_array[n] / self.__m_max_abs))


class FrequencyModulator(SignalGenerator):
    def __init__(self, length, arrays):
        super().__init__(length, -1)
        self.__arrays = arrays  # [[]] == [[chart1 values], [chart2 values] ...]

    def get_discrete_signal(self, n):
        return 0

# class MutationType(Enum):
#     INCREMENT = 1
#     DECREMENT = -1
#
#
# HarmonicMutations = namedtuple('HarmonicMutations', ['amplitude_mutation',
#                                                      'frequency_mutation',
#                                                      'initial_phase_mutation'])
#
#
# class LinearPolyharmonicSignalGenerator:
#     def __init__(self, harmonic_params_collection):
#         self.__harmonic_params_collection = harmonic_params_collection
#
#     def generate_signal(self, period, period_iterations, mutation_per_period, mutation_type):
#
#         mutation_per_signal_part = mutation_per_period / period
#         mutation_multiplier = mutation_type.value
#         mutations = []
#
#         for harmonic_params in self.__harmonic_params_collection:
#             mutations.append(
#                 HarmonicMutations(
#                     harmonic_params.amplitude * mutation_per_signal_part * mutation_multiplier,
#                     harmonic_params.frequency * mutation_per_signal_part * mutation_multiplier,
#                     harmonic_params.initial_phase * mutation_per_signal_part * mutation_multiplier))
#
#         for n in range(period * period_iterations):
#             new_harmonic_params_collection = []
#             for index, harmonic_params in enumerate(self.__harmonic_params_collection):
#                 new_harmonic_params_collection.append(
#                     HarmonicParameters(
#                         harmonic_params.amplitude + mutations[index].amplitude_mutation,
#                         harmonic_params.frequency + mutations[index].frequency_mutation,
#                         harmonic_params.initial_phase + mutations[index].initial_phase_mutation))
#
#             self.__harmonic_params_collection = new_harmonic_params_collection
#
#             yield PolyharmonicSignalGenerator(self.__harmonic_params_collection).get_discrete_signal(n, period)
#
#
# class HarmonicSignalGenerator2:
#     def __init__(self, initial_phase, k_generator):
#         self.__initial_phase = initial_phase
#         self.__k_generator = k_generator
#
#     def __get_signal_part(self, n, period):
#         return math.sin(2 * math.pi * n / period + self.__initial_phase)
#
#     def __get_signal(self, m, period):
#         for n in range(m):
#             yield self.__get_signal_part(n, period)
#
#     def get_signals(self, period):
#         m_step = math.floor(period / 4 / 8)
#         m_collection = array('i')
#         k = self.__k_generator(period)
#         for cur_m in range(period, k, -m_step):
#             m_collection.append(cur_m)
#             cur_m -= m_step
#         m_collection.reverse()
#         for cur_m in range(period + m_step, 2 * period, m_step):
#             m_collection.append(cur_m)
#
#         signals = []
#         for m in m_collection:
#             signals.append(self.__get_signal(m, period))
#
#         return signals
#
#
# class HarmonicSignalAnalyzer:
#     def __init__(self, harmonic_signal):
#         self.__signal = harmonic_signal
#
#     def __get_sum(self):
#         return sum(self.__signal)
#
#     def __get_sum_of_squared(self):
#         return sum([signal_part ** 2 for signal_part in self.__signal])
#
#     def get_root_mean_square_value(self):
#         return math.sqrt(self.__get_sum_of_squared() / len(self.__signal))
#
#     def get_standard_deviation(self):
#         signals_count = len(self.__signal)
#         return math.sqrt(self.__get_sum_of_squared() / signals_count - (self.__get_sum() / signals_count) ** 2)
#
#     def get_amplitude(self):
#         max_amplitude = 0
#         signal_length = len(self.__signal)
#         for k in range(math.floor(signal_length / 2)):
#             complex_amplitude = complex()
#             for n, x in enumerate(self.__signal):
#                 trigonometric_arg = 2 * math.pi * k * n / signal_length
#                 complex_amplitude += x * (math.cos(trigonometric_arg) + 1j * math.sin(trigonometric_arg))
#             max_amplitude = max(max_amplitude, 2 * math.sqrt(complex_amplitude.real ** 2 + complex_amplitude.imag ** 2)
#                                 / signal_length)
#         return max_amplitude
#
#     def get_root_mean_square_value_error(self):
#         return 0.707 - self.get_root_mean_square_value()
#
#     def get_amplitude_error(self):
#         return 1 - self.get_amplitude()
