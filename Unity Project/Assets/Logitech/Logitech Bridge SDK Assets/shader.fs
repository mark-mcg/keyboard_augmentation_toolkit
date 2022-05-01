#version 330

in vec3 FragPos;  
in vec3 Normal;
in vec2 Texcoord;

uniform vec3 lightPos; 
uniform vec3 viewPos;
uniform vec3 lightColor;
out vec4 outColor;

uniform sampler2D mainTexture;
uniform sampler2D ambientOcclusion;
uniform sampler2D normalMap;

void main()
{
	vec4 objectColor = texture(mainTexture, Texcoord);

	// Ambient
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
  	
    // Diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
    
    // Specular
    float specularStrength = 0.6f;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;  
        
    outColor = vec4((ambient + diffuse + specular),1.0) * objectColor + vec4( texture(ambientOcclusion, Texcoord).rgb * 0.1 , 1.0);
}
