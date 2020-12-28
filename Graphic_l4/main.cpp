#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include "stb_image.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "shader.h"
#include "camera.h"
#include "model.h"

#include <iostream>

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void mouse_callback(GLFWwindow* window, double xpos, double ypos);
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
void processInput(GLFWwindow *window);
unsigned int loadTexture(const char *path);
void renderFloor(const Shader &shader);
void renderWalls(const Shader& shader);
void renderCeiling(const Shader& shader);
void renderTable(const Shader& shader, Model& officeTableModel);
void renderChair(const Shader& shader, Model& officeChairModel);
void renderBossChair(const Shader& shader, Model& bossChairModel);
void renderSofa(const Shader& shader, Model& sofaModel);
void renderDoor(const Shader& shader, Model& doorModel);
void renderLustre(const Shader& shader, Model& lustreModel);

// настройки
const unsigned int SCR_WIDTH = 600;
const unsigned int SCR_HEIGHT = 400;

// камера
Camera camera(glm::vec3(0.0f, 0.5f, 3.0f));
float lastX = (float)SCR_WIDTH / 2.0;
float lastY = (float)SCR_HEIGHT / 2.0;
bool firstMouse = true;

// тайминги
float deltaTime = 0.0f;
float lastFrame = 0.0f;

// меши
unsigned int planeVAO;


int main()
{
    // glfw: инициализация и конфигурирование
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);


    // glfw: создание окна
    GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "Graphic Lab4", NULL, NULL);
    if (window == NULL)
    {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
    glfwSetCursorPosCallback(window, mouse_callback);
    glfwSetScrollCallback(window, scroll_callback);

    // говорим GLFW захватить курсор нашей мышки
    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

    // glad: загрузка всех указателей на OpenGL-функции
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
    {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }

    // конфигурирование глобального состояния OpenGL
    glEnable(GL_DEPTH_TEST);

    // компилирование нашей шейдерной программы
    Shader shader("3.1.3.shadow_mapping.vs", "3.1.3.shadow_mapping.fs");
    Shader simpleDepthShader("3.1.3.shadow_mapping_depth.vs", "3.1.3.shadow_mapping_depth.fs");

    // установка вершинных данных (буффера(-ов)) и настройка вершинных атрибутов
    // ------------------------------------------------------------------
    float planeVertices[] = {
        // координаты            // нормали         // текстурные координаты
         8.0f, -0.5f,  8.0f,  0.0f, 1.0f, 0.0f,   8.0f,  0.0f,
        -8.0f, -0.5f,  8.0f,  0.0f, 1.0f, 0.0f,   0.0f,  0.0f,
        -8.0f, -0.5f, -8.0f,  0.0f, 1.0f, 0.0f,   0.0f,  8.0f,

         8.0f, -0.5f,  8.0f,  0.0f, 1.0f, 0.0f,   8.0f,  0.0f,
        -8.0f, -0.5f, -8.0f,  0.0f, 1.0f, 0.0f,   0.0f,  8.0f,
         8.0f, -0.5f, -8.0f,  0.0f, 1.0f, 0.0f,   8.0f,  8.0f
    };

    //VAO пола
    unsigned int planeVBO;
    glGenVertexArrays(1, &planeVAO);
    glGenBuffers(1, &planeVBO);
    glBindVertexArray(planeVAO);
    glBindBuffer(GL_ARRAY_BUFFER, planeVBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(planeVertices), planeVertices, GL_STATIC_DRAW);
    glEnableVertexAttribArray(0);
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(1);
    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
    glEnableVertexAttribArray(2);
    glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(6 * sizeof(float)));
    glBindVertexArray(0);



    // загрузка текстур
    unsigned int woodTexture = loadTexture("textures/wood.png");
    unsigned int yellowTexture = loadTexture("textures/magenta.png");
    unsigned int tableTexture = loadTexture("textures/desk.jpg ");
    unsigned int wallTexture = loadTexture("textures/wall.jpg");
    unsigned int ceilingTexture = loadTexture("textures/white.jpg");



    //настройка глубины
    const unsigned int SHADOW_WIDTH = 1024, SHADOW_HEIGHT = 1024;
    unsigned int depthMapFBO;
    glGenFramebuffers(1, &depthMapFBO);
    unsigned int depthMap;
    glGenTextures(1, &depthMap);
    glBindTexture(GL_TEXTURE_2D, depthMap);
    glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, SHADOW_WIDTH, SHADOW_HEIGHT, 0, GL_DEPTH_COMPONENT, GL_FLOAT, NULL);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
    float borderColor[] = { 1.0, 1.0, 1.0, 1.0 };
    glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

    // прикрепляем текстуру глубины в качестве буфера глубины
    glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
    glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);
    glDrawBuffer(GL_NONE);
    glReadBuffer(GL_NONE);
    glBindFramebuffer(GL_FRAMEBUFFER, 0);


    // конфигурация шейдеров
    shader.use();
    shader.setInt("diffuseTexture", 0);
    shader.setInt("shadowMap", 1);

    // параметры освещения
    glm::vec3 lightPos(0.0f, 2.0f, 1.5f);

    //создание и инициализация моделей
    Model bossChairModel("ModelOfficeChair/10259_Wingback_Chair_v2_max2011_it1.obj");
    Model officeChairModel("ModelChair/Artichoke.obj");
    Model officeTableModel("ModelTable/10240_Office_Desk_v3_max2011.obj");
    Model sofaModel("ModelSofa/HSM0012.obj");
    Model doorModel("ModelDoor/10057_wooden_door_v3_iterations-2.obj");
    Model lustreModel("LightModel/MODUL_CHANDELIER.obj");

    // цикл рендеринга
    while (!glfwWindowShouldClose(window))
    {
        // логическая часть работы со временем для каждого кадра
        float currentFrame = glfwGetTime();
        deltaTime = currentFrame - lastFrame;
        lastFrame = currentFrame;

        // обработка ввода
        processInput(window);

        // рендер
        glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        // 1. рендеринг глубины сцены в текстуру (вид - с позиции источника света)
        glm::mat4 lightProjection, lightView;
        glm::mat4 lightSpaceMatrix;
        float near_plane = 1.0f, far_plane = 8.5f;
        lightProjection = glm::ortho(-5.0f, 5.0f, -1.0f, 5.0f, near_plane, far_plane);
        lightView = glm::lookAt(lightPos, glm::vec3(0.0f), glm::vec3(0.0, 0.0, 2.0));
        lightSpaceMatrix = lightProjection * lightView;
        // рендеринг сцены глазами источника света
        simpleDepthShader.use();
        simpleDepthShader.setMat4("lightSpaceMatrix", lightSpaceMatrix);

        glViewport(0, 0, SHADOW_WIDTH, SHADOW_HEIGHT);
        glBindFramebuffer(GL_FRAMEBUFFER, depthMapFBO);
        glClear(GL_DEPTH_BUFFER_BIT);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, woodTexture);
        renderFloor(simpleDepthShader);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, wallTexture);
        renderWalls(simpleDepthShader);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, ceilingTexture);
        renderCeiling(simpleDepthShader);


        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, yellowTexture);
        renderChair(simpleDepthShader, officeChairModel);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, tableTexture);
        renderTable(simpleDepthShader, officeTableModel);
        renderBossChair(simpleDepthShader, bossChairModel);
        renderSofa(simpleDepthShader, sofaModel);
        renderDoor(simpleDepthShader, doorModel);
        renderLustre(simpleDepthShader, lustreModel);


        glBindFramebuffer(GL_FRAMEBUFFER, 0);

        // сброс настроек области просмотра
        glViewport(0, 0, SCR_WIDTH, SCR_HEIGHT);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        // 2. рендеринг сцену как обычно, но используем при этом сгенерированную карту глубины/тени 
        glViewport(0, 0, SCR_WIDTH, SCR_HEIGHT);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        shader.use();
        glm::mat4 projection = glm::perspective(glm::radians(camera.Zoom), (float)SCR_WIDTH / (float)SCR_HEIGHT, 0.1f, 100.0f);
        glm::mat4 view = camera.GetViewMatrix();
        shader.setMat4("projection", projection);
        shader.setMat4("view", view);

        // устанавливаем uniform-переменные для освещения
        shader.setVec3("viewPos", camera.Position);
        shader.setVec3("lightPos", lightPos);
        shader.setMat4("lightSpaceMatrix", lightSpaceMatrix);

        //отрисовываем объекты сцены
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, woodTexture);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, depthMap);
        renderFloor(shader);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, wallTexture);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, depthMap);
        renderWalls(shader);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, ceilingTexture);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, depthMap);
        renderCeiling(shader);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, yellowTexture);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, depthMap);
        renderChair(shader, officeChairModel);

        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, tableTexture);
        glActiveTexture(GL_TEXTURE1);
        glBindTexture(GL_TEXTURE_2D, depthMap);
        renderTable(shader, officeTableModel);
        renderBossChair(shader, bossChairModel);
        renderSofa(shader, sofaModel);
        renderDoor(shader, doorModel);
        renderLustre(shader, lustreModel);


        // glfw: обмен содержимым переднего и заднего буферов. Опрос событий Ввода\Вывода
        glfwSwapBuffers(window);
        glfwPollEvents();
    }

    // опционально: освобеждение памяти, выделенной под ресурсы
    glDeleteVertexArrays(1, &planeVAO);
    glDeleteBuffers(1, &planeVBO);

    glfwTerminate();
    return 0;
}

//рендеринг пола
void renderFloor(const Shader &shader)
{
    glm::mat4 model = glm::mat4(1.0f);
    shader.setMat4("model", model);
    glBindVertexArray(planeVAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);
}

//рендеринг стен
void renderWalls(const Shader& shader)
{
    glm::mat4 model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, 0.0f, -2.0f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    model = glm::scale(model, glm::vec3(0.5f));
    shader.setMat4("model", model);
    glBindVertexArray(planeVAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);

    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, 0.0f, 5.05f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    model = glm::scale(model, glm::vec3(0.5f));
    shader.setMat4("model", model);
    glBindVertexArray(planeVAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);

    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(2.5f, 0.0f, 2.0f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(0.0, 0.0, 1.0)));
    model = glm::scale(model, glm::vec3(0.5f));
    shader.setMat4("model", model);
    glBindVertexArray(planeVAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);

    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(-3.0f, 0.0f, 2.0f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(0.0, 0.0, 1.0)));
    model = glm::scale(model, glm::vec3(0.5f));
    shader.setMat4("model", model);
    glBindVertexArray(planeVAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);
}

//рендеринг потолка
void renderCeiling(const Shader& shader)
{
    glm::mat4 model = glm::mat4(1.0f);
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, 3.0f, 0.0f));
    shader.setMat4("model", model);
    glBindVertexArray(planeVAO);
    glDrawArrays(GL_TRIANGLES, 0, 6);
    glBindVertexArray(0);
}

//рендеринг стола
void renderTable(const Shader& shader, Model& officeTableModel)
{
    glm::mat4 model;
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, -0.5f, 0.0f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    model = glm::rotate(model, glm::radians(180.0f), glm::normalize(glm::vec3(0.0, 0.0, 1.0)));
    model = glm::scale(model, glm::vec3(0.01f));
    shader.setMat4("model", model);
    officeTableModel.Draw(shader);

}

//рендеринг кресл
void renderBossChair(const Shader& shader, Model& bossChairModel)
{
    glm::mat4 model;
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, -0.5f, -1.0f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    model = glm::scale(model, glm::vec3(0.01f));
    shader.setMat4("model", model);
    bossChairModel.Draw(shader);

}

//рендеринг стульев
void renderChair(const Shader& shader, Model& officeChairModel)
{
    glm::mat4 model;
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(-2.0f, -0.5f, 2.0f));
    model = glm::scale(model, glm::vec3(0.01f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(0.0, 1.0, 0.0)));
    shader.setMat4("model", model);
    officeChairModel.Draw(shader);

    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(2.0f, -0.5f, 2.0f));
    model = glm::scale(model, glm::vec3(0.01f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(0.0, -1.0, 0.0)));
    shader.setMat4("model", model);
    officeChairModel.Draw(shader);
}

//рендеринг дивана
void renderSofa(const Shader& shader, Model& sofaModel)
{
    glm::mat4 model;
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(-1.5f, -0.5f, 4.75f));
    model = glm::scale(model, glm::vec3(0.01f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(0.0, 1.0, 0.0)));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(0.0, 0.0, 1.0)));
    shader.setMat4("model", model);
    sofaModel.Draw(shader);
}

//рендеринг двери
void renderDoor(const Shader& shader, Model& doorModel)
{
    glm::mat4 model;
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(1.5f, -0.5f, 5.3f));
    model = glm::scale(model, glm::vec3(0.01f));
    model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    shader.setMat4("model", model);
    doorModel.Draw(shader);
}

//рендеринг люстры
void renderLustre(const Shader& shader, Model& lustreModel)
{
    glm::mat4 model;
    model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, 1.95f, 1.5f));
    model = glm::scale(model, glm::vec3(0.005f));
    //model = glm::rotate(model, glm::radians(90.0f), glm::normalize(glm::vec3(-1.0, 0.0, 0.0)));
    shader.setMat4("model", model);
    lustreModel.Draw(shader);
}


//обработка всех событий ввода
void processInput(GLFWwindow *window)
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
        glfwSetWindowShouldClose(window, true);

    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
        camera.ProcessKeyboard(FORWARD, deltaTime);
    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
        camera.ProcessKeyboard(BACKWARD, deltaTime);
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
        camera.ProcessKeyboard(LEFT, deltaTime);
    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
        camera.ProcessKeyboard(RIGHT, deltaTime);
}

// glfw: всякий раз, когда изменяются размеры окна (пользователем или опер. системой), вызывается данная функция
void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    // убеждаемся, что вьюпорт соответствует новым размерам окна; обратите внимание,
    // что ширина и высота будут значительно больше, чем указано на retina -дисплеях.
    glViewport(0, 0, width, height);
}

// glfw: всякий раз, когда перемещается мышь, вызывается данная callback-функция
void mouse_callback(GLFWwindow* window, double xpos, double ypos)
{
    if (firstMouse)
    {
        lastX = xpos;
        lastY = ypos;
        firstMouse = false;
    }

    float xoffset = xpos - lastX;
    float yoffset = lastY - ypos; // перевернуто, так как Y-координаты идут снизу вверх

    lastX = xpos;
    lastY = ypos;

    camera.ProcessMouseMovement(xoffset, yoffset);
}

// glfw: всякий раз, когда прокручивается колесико мыши, вызывается данная callback-функция
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
{
    camera.ProcessMouseScroll(yoffset);
}

// вспомогательная функция загрузки 2D-текстур из файла
unsigned int loadTexture(char const * path)
{
    unsigned int textureID;
    glGenTextures(1, &textureID);

    int width, height, nrComponents;
    unsigned char *data = stbi_load(path, &width, &height, &nrComponents, 0);
    if (data)
    {
        GLenum format;
        if (nrComponents == 1)
            format = GL_RED;
        else if (nrComponents == 3)
            format = GL_RGB;
        else if (nrComponents == 4)
            format = GL_RGBA;

        glBindTexture(GL_TEXTURE_2D, textureID);
        glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
        glGenerateMipmap(GL_TEXTURE_2D);

        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, format == GL_RGBA ? GL_CLAMP_TO_EDGE : GL_REPEAT); // для данного урока: используйте GL_CLAMP_TO_EDGE для предотвращения возникновения полупрозрачных границ. Благодаря интерполяции берутся тексели из следующего повторения
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, format == GL_RGBA ? GL_CLAMP_TO_EDGE : GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

        stbi_image_free(data);
    }
    else
    {
        std::cout << "Texture failed to load at path: " << path << std::endl;
        stbi_image_free(data);
    }

    return textureID;
}
