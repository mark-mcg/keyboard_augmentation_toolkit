#version 330

in vec3 position;
in vec3 normal;
in vec2 texcoord;
out vec3 Normal;
out vec3 FragPos;
out vec2 Texcoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{

    Texcoord = texcoord;
	FragPos = vec3(model * vec4(position, 1.0f));
    Normal = mat3(transpose(inverse(model))) * normal;
    gl_Position = projection * view * model * vec4(position, 1.0);
}