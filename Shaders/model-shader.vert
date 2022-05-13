#version 120

attribute vec3 a_Position;
attribute vec2 a_TexCoord;

uniform mat4 u_Projection;

varying vec2 v_TexCoord;

void main()
{
    vec3 pos = a_Position + vec3(0, 0, -5);
    gl_Position = u_Projection * vec4(pos, 1);
    v_TexCoord = a_TexCoord;
}