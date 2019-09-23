--------------------------------------------------------------------------------
-- Copyright (c) 1995-2013 Xilinx, Inc.  All rights reserved.
--------------------------------------------------------------------------------
--   ____  ____
--  /   /\/   /
-- /___/  \  /    Vendor: Xilinx
-- \   \   \/     Version: P.20131013
--  \   \         Application: netgen
--  /   /         Filename: mux_synthesis.vhd
-- /___/   /\     Timestamp: Mon Sep 23 23:36:56 2019
-- \   \  /  \ 
--  \___\/\___\
--             
-- Command	: -intstyle ise -ar Structure -tm mux -w -dir netgen/synthesis -ofmt vhdl -sim mux.ngc mux_synthesis.vhd 
-- Device	: xc6slx9-3-ftg256
-- Input file	: mux.ngc
-- Output file	: E:\test\netgen\synthesis\mux_synthesis.vhd
-- # of Entities	: 1
-- Design Name	: mux
-- Xilinx	: C:\Xilinx\14.7\ISE_DS\ISE\
--             
-- Purpose:    
--     This VHDL netlist is a verification model and uses simulation 
--     primitives which may not represent the true implementation of the 
--     device, however the netlist is functionally correct and should not 
--     be modified. This file cannot be synthesized and should only be used 
--     with supported simulation tools.
--             
-- Reference:  
--     Command Line Tools User Guide, Chapter 23
--     Synthesis and Simulation Design Guide, Chapter 6
--             
--------------------------------------------------------------------------------

library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
library UNISIM;
use UNISIM.VCOMPONENTS.ALL;
use UNISIM.VPKG.ALL;

entity mux is
  port (
    A : in STD_LOGIC := 'X'; 
    B : in STD_LOGIC := 'X'; 
    S : in STD_LOGIC := 'X'; 
    Z : out STD_LOGIC 
  );
end mux;

architecture Structure of mux is
  signal A_IBUF_0 : STD_LOGIC; 
  signal B_IBUF_1 : STD_LOGIC; 
  signal S_IBUF_2 : STD_LOGIC; 
  signal Z_OBUF_3 : STD_LOGIC; 
begin
  Mmux_Z11 : LUT3
    generic map(
      INIT => X"E4"
    )
    port map (
      I0 => S_IBUF_2,
      I1 => A_IBUF_0,
      I2 => B_IBUF_1,
      O => Z_OBUF_3
    );
  A_IBUF : IBUF
    port map (
      I => A,
      O => A_IBUF_0
    );
  B_IBUF : IBUF
    port map (
      I => B,
      O => B_IBUF_1
    );
  S_IBUF : IBUF
    port map (
      I => S,
      O => S_IBUF_2
    );
  Z_OBUF : OBUF
    port map (
      I => Z_OBUF_3,
      O => Z
    );

end Structure;

