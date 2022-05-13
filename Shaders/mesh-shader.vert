#version 120

attribute vec2 a_Position;
attribute vec4 a_Color;

uniform mat4 u_Projection;
uniform mat4 u_Model;

varying vec4 v_Color;

void main(void)
{
    gl_Position = u_Projection * u_Model * vec4(a_Position, -30, 1);
    v_Color = a_Color;
}