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

    def __init__(self, parser):
        self.__parser = parser
        self.__args = []
        self.__tasks_callbacks = {}

    def get_length(self):
        self.__parser.add_argument('-N', '--length',
                                   action='store',
                                   required=False,
                                   help='signal length',
                                   dest='length',
                                   type=int,
                                   default=512)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.length

    def get_amplitudes(self, same=1):
        # total parameters passed after -A:
        #   -A 1 2 .. same-1 same
        self.__parser.add_argument('-A', '--amplitude',
                                   action='store',
                                   required=False,
                                   help='signal amplitude',
                                   dest='amplitude',
                                   type=float,
                                   nargs=same)
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

    def get_frequencies(self, same=1):
        self.__parser.add_argument('-f', '--frequency',
                                   action='store',
                                   required=False,
                                   help='frequency',
                                   dest='frequency',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.frequency

    def get_initial_phases(self, same=1):
        self.__parser.add_argument('-p', '--initial-phase',
                                   action='store',
                                   required=False,
                                   help='initial phase',
                                   dest='initial_phase',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.initial_phase

    def get_iterations(self):
        self.__parser.add_argument('-i', '--iterations',
                                   action='store',
                                   required=False,
                                   help='total iterations',
                                   dest='iterations',
                                   type=int,
                                   default=5)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.iterations

    def get_duty_circles(self, same=1):
        self.__parser.add_argument('-D', '--duty-circle',
                                   action='store',
                                   required=False,
                                   help='signal length percentage',
                                   dest='duty_circle',
                                   type=float,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.duty_circle

    def get_growings(self, same=1):
        self.__parser.add_argument('-g', '--growing',
                                   action='store',
                                   required=False,
                                   help='chart direction 0(desc) or 1(asc)',
                                   dest='growing',
                                   type=int,
                                   nargs=same)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.growing

    def get_nargs(self):
        self.__parser.add_argument('-z', '--nargs',
                                   action='store',
                                   required=False,
                                   help='same args total',
                                   dest='nargs',
                                   type=int,
                                   default=1)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.nargs

    def get_is_polyharmonic(self) -> bool:
        # 0 is usual vs. 1 is polyharmonic
        self.__parser.add_argument('-y', '--is-poly',
                                   action='store',
                                   required=False,
                                   help='whether signal is polyharmonic',
                                   dest='is_poly',
                                   type=int,
                                   default=0)
        self.__args = self.__parser.parse_known_args()[0]
        return self.__args.is_poly == 1

