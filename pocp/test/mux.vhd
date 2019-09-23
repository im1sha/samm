LIBRARY ieee;			-- Use IEEE library
USE ieee.std_logic_1164.all;		-- IEEE packages

ENTITY mux IS			-- Entity of multiplexer
   port(				-- Ports description
      A,B,S: in std_logic;		-- A and B are logical inputs
				-- S is the control input signal
      Z: out std_logic			-- Z is a logical output
   );
END mux;
--
ARCHITECTURE beh OF mux IS	-- Architecture of multiplexer
BEGIN
   Z<=A when S='0' else B;		-- Behavioural description by logic equation
END beh;


