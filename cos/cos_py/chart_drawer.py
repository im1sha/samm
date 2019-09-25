from matplotlib import pyplot
from collections import namedtuple

LabeledChartData = namedtuple('LabeledChartData', ['indexes',
                                                   'values',
                                                   'label'])


def draw_charts(chart_data_collection):
    pyplot.rcParams['toolbar'] = 'None'

    for chart_data in chart_data_collection:
        pyplot.plot(chart_data.indexes, chart_data.values, label=chart_data.label)

    pyplot.legend()
    pyplot.grid(True)
    pyplot.show()


def draw_chart(chart_data):
    draw_charts([chart_data])
