#version 330 core

uniform vec3 objectColor;

out vec4 FragColor;

in vec3 Normal;

void main()
{
	vec3 norm = normalize(Normal);
	vec3 lightDir = vec3(0.0, -1.0, 1.0);
	float diffuse = dot(norm, lightDir);

	FragColor = vec4(diffuse * objectColor, 1.0);
}