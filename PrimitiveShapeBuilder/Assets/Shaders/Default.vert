#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aText;
layout(location = 2) in vec3 aNorm;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 UV;
out vec3 Normal;
out vec3 FragPos;

void main()
{
	gl_Position = vec4(aPos, 1.0) * model * view * projection;
	UV = aText;
	Normal = aNorm;
	FragPos = vec3(vec4(aPos, 1.0) * model);
}