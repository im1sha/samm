import argparse
from chart_drawer import *
from signal_generators import *
from local_parser import LocalParser


# harmonic| impulse| triangle| saw_edged| noise functions return simple signals collection
# [[signal1], [signal2], [signalNargs]]

def harmonic(initial_phases):
    return list(list(HarmonicSignalGenerator(p).generate_signal()) for p in initial_phases)


def impulse(duty_circles):
    return list(list(ImpulseSignalGenerator(d).generate_signal()) for d in duty_circles)


def triangle(n):
    return list(list(TriangleSignalGenerator().generate_signal()) for _ in range(n))


def saw_edged(growings):
    return list(list(SawEdgedSignalGenerator(g).generate_signal()) for g in growings)


def noise(n):
    return list(list(NoiseSignalGenerator().generate_signal()) for _ in range(n))


# only calls appropriate signal creation function

# def poly(task, nargs):
#     arrays = task(nargs)
#     return [list(PolyharmonicSignalGenerator(len(arrays[0]), arrays).generate_signal())]


# def f_modulate(tasks):
#     print('fm')


# m_task values - [information signal]
# c_task values - [carrier signal]

# def a_modulate(m_task, c_task):
#     m_values = m_task(1, False, False)[0]
#     c_values = c_task(1, False, True)[0]
#     if len(m_values) != len(c_values):
#         raise Exception()
#     return [list(AmplitudeModulator(len(m_values), m_values, c_values).generate_signal()), m_values, c_values]

def main():
    signals_callbacks = {
        'harmonic': harmonic,
        'impulse': impulse,
        'triangle': triangle,
        'sawedged': saw_edged,
        'noise': noise
    }
    # modulate_callbacks = {
    #     'fm': f_modulate,
    #     'am': a_modulate,
    #     'none': None
    # }

    parser = LocalParser(argparse.ArgumentParser())
    #modulation = parser.get_modulation(modulate_callbacks)

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
        # harmonic
        #initial_phases = parser.get_initial_phases(nargs, store_last_group, only_last, exact_at)  # []

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
