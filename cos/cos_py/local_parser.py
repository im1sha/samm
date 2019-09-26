import itertools


class LocalParser:
    # use for single signal generation
    # __main__.py -t SIGNAL_NAME_HERE --arg1-key ARG1_VALUE
    #                                 --arg2-key ARG2_VALUE
    #                                 --argm-key ARGM_VALUE
    #                                 -i TOTAL_ITERATIONS
    # use for multiple signal generation
    # __main__.py -t SIGNAL_NAME_HERE --arg1-key ARG1_VALUE_1 ARG1_VALUE_2 ARG1_VALUE_N
    #                                 --arg2-key ARG2_VALUE_1 ARG2_VALUE_2 ARG2_VALUE_N
    #                                 --argm-key ARGM_VALUE_1 ARGM_VALUE_2 ARGM_VALUE_N
    #                                 -i TOTAL_ITERATIONS
    #                                 -z TOTAL_SIGNALS_N
    # use for polysignal generation
    # __main__.py -t SIGNAL_NAME_HERE --arg1-key ARG1_VALUE_1 ARG1_VALUE_2 ARG1_VALUE_N
    #                                 --arg2-key ARG1_VALUE_1 ARG1_VALUE_2 ARG1_VALUE_N
    #                                 --argm-key ARGM_VALUE_1 ARGM_VALUE_2 ARGM_VALUE_N
    #                                 -i TOTAL_ITERATIONS
    #                                 -z TOTAL_SIGNALS_N
    #                                 -y 1
    # use for modulation
    # __main__.py -t SIGNAL_NAME_1 --arg1-key ARG1_VALUE
    #                              --arg2-key ARG1_VALUE
    #                              --argm-key ARGM_VALUE
    #             -t SIGNAL_NAME_2 --arg1-key ARG1_VALUE
    #                              --arg2-key ARG1_VALUE
    #                              --argl-key ARGL_VALUE
    #                              -i TOTAL_ITERATIONS
    #                              -z TOTAL_SIGNALS_N
    #                              -m fm | am

    def __init__(self, parser):
        self.__parser = parser
        self.__args = []
        self.__tasks_callbacks = {}
        self.__modulation = None

    def __to_1d_array(self, value):
        def __get_nest_level(obj):
            if type(obj) != list:
                return 0
            max_level = 0
            for item in obj:
                max_level = max(max_level, __get_nest_level(item))
            return max_level + 1

        if __get_nest_level(value) == 0:
            return [value]
        if __get_nest_level(value) == 1:
            return value
        else:
            return list(itertools.chain.from_iterable(value))

    def __get_value_at(self, value, pos=0):
        return self.__to_1d_array(value)[pos]

    # [val]
    # USE: --key VALUE

    # def __arg(self):

    def get_length(self):
        self.__parser.add_argument('-N', '--length',
                                   action='store',
                                   required=False,
                                   help='signal length',
                                   dest='length',
                                   type=int,
                                   default=512,
                                   nargs=1)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__get_value_at(self.__args.length)

    def get_iterations(self):
        self.__parser.add_argument('-i', '--iterations',
                                   action='store',
                                   required=False,
                                   help='total iterations',
                                   dest='iterations',
                                   type=int,
                                   default=1,
                                   nargs=1)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__get_value_at(self.__args.iterations)

    def get_nargs(self):
        # for multiple signals generation
        self.__parser.add_argument('-z', '--nargs',
                                   action='store',
                                   required=False,
                                   help='same args total',
                                   dest='nargs',
                                   type=int,
                                   default=1,
                                   nargs=1)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__get_value_at(self.__args.nargs)

    # store == False, same == 1
    # [[value1], [value2], [valueN]]
    # USE       --key VALUE_1 --key VALUE_2 --key VALUE_N

    # store == True, same >= 1
    # [value1, value2, valueN]
    # USE       --key VALUE_1 VALUE_2 VALUE_N
    #           --nargs N

    def get_amplitudes(self, same=1, store=True, only_last_to_take=False):
        # total parameters passed after -A:
        #   -A 1 2 .. same-1 same
        # item_to_take is number of satisfying item (-1: take all)
        if (same != 1) and not store:
            raise Exception()
        self.__parser.add_argument('-A', '--amplitude',
                                   action='store' if store else 'append',
                                   required=False,
                                   help='signal amplitude',
                                   dest='amplitude',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        if self.__args.amplitude is None:
            raise Exception()
        result = self.__to_1d_array(self.__args.amplitude)
        if only_last_to_take:
            return [result[len(result)-1]]
        return result

    def get_frequencies(self, same=1, store=True, only_last_to_take=False):
        if (same != 1) and not store:
            raise Exception()
        self.__parser.add_argument('-f', '--frequency',
                                   action='store' if store else 'append',
                                   required=False,
                                   help='frequency',
                                   dest='frequency',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        if self.__args.frequency is None:
            raise Exception()
        result = self.__to_1d_array(self.__args.frequency)
        if only_last_to_take:
            return [result[len(result) - 1]]
        return result

    def get_initial_phases(self, same=1, store=True, only_last_to_take=False):
        if (same != 1) and not store:
            raise Exception()
        self.__parser.add_argument('-p', '--initial-phase',
                                   action='store' if store else 'append',
                                   required=False,
                                   help='initial phase',
                                   dest='initial_phase',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        if self.__args.initial_phase is None:
            raise Exception()
        result = self.__to_1d_array(self.__args.initial_phase)
        if only_last_to_take:
            return [result[len(result) - 1]]
        return result

    def get_duty_circles(self, same=1, store=True, only_last_to_take=False):
        if (same != 1) and not store:
            raise Exception()
        self.__parser.add_argument('-D', '--duty-circle',
                                   action='store' if store else 'append',
                                   required=False,
                                   help='signal length percentage',
                                   dest='duty_circle',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        if self.__args.duty_circle is None:
            raise Exception()
        result = self.__to_1d_array(self.__args.duty_circle)
        if only_last_to_take:
            return [result[len(result) - 1]]
        return result

    def get_growings(self, same=1, store=True, only_last_to_take=False):
        if (same != 1) and not store:
            raise Exception()
        self.__parser.add_argument('-g', '--growing',
                                   action='store' if store else 'append',
                                   required=False,
                                   help='chart direction 0(desc) or 1(asc)',
                                   dest='growing',
                                   type=int,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        if self.__args.growing is None:
            raise Exception()
        result = self.__to_1d_array(self.__args.growing)
        if only_last_to_take:
            return [result[len(result) - 1]]
        return result

    def get_tasks(self, tasks_callbacks, store=True):
        self.__tasks_callbacks = tasks_callbacks
        self.__parser.add_argument('-t', '--task',
                                   action='store' if store else 'append',
                                   required=True,
                                   help='task name',
                                   choices=tasks_callbacks.keys(),
                                   dest='task',
                                   type=str,
                                   nargs=1)
        self.__args = self.__parser.parse_known_args()[0]
        results = []
        for i in self.__to_1d_array(self.__args.task):
            if i in tasks_callbacks.keys():
                results.append(tasks_callbacks.get(i))

        return results

    #

    def get_is_polyharmonic(self) -> bool:
        # 0 is usual vs. 1 is polyharmonic
        self.__parser.add_argument('-y', '--is-poly',
                                   action='store',
                                   required=False,
                                   help='whether signal is polyharmonic',
                                   dest='is_poly',
                                   type=int,
                                   default=0,
                                   nargs=1)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__get_value_at(self.__args.is_poly) == 1

    def get_modulation(self, modulation_callbacks):
        self.__parser.add_argument('-m', '--modulation',
                                   action='store',
                                   required=False,
                                   help='modulation kind/absence of it',
                                   choices=modulation_callbacks.keys(),
                                   dest='modulation',
                                   type=str,
                                   default='none',
                                   nargs=1)
        self.__args = self.__parser.parse_known_args()[0]
        self.__modulation = modulation_callbacks[self.__get_value_at(self.__args.modulation)]
        return self.__modulation
