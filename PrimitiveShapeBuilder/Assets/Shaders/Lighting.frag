#version 330 core

out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 objectColor;

in vec3 Normal;
in vec3 FragPos;

void main()
{
	float ambientStrength = 0.05;

	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diffuse = dot(norm, lightDir);

	float lightDist = max(distance(lightPos, FragPos) / 5.0, 1.0);

	FragColor = vec4((ambientStrength + diffuse) / lightDist * objectColor, 1.0);
}