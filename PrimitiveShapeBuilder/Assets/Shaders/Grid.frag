#version 330 core

out vec4 FragColor;

uniform vec3 cameraPosition;

in vec2 UV;

float fade(vec2 st, float radius)
{
	vec2 dist = st - vec2(0.5);
	return 1.0 - smoothstep(0.0, radius, dot(dist, dist) * 3.14);
}

float grid(vec2 size)
{
	size = vec2(0.5) - size;
	vec2 uv = floor(size);
	return uv.x * uv.y;
}

void main()
{
	vec2 st = UV;
	vec3 cam = cameraPosition;

	vec3 fadeColor = 1.0 - vec3(fade(st, 1.0));

	cam = cam * 1.620; // for the weird parallax problem

	st = fract(st * 32 + vec2(cam.x, -cam.z));
	vec3 gridColor = 1.0 - vec3(grid(vec2(st * 6.0)));

	FragColor = vec4(1.0, 1.0, 1.0, (gridColor - fadeColor) / (3 + abs(cam.y / 2)));
}