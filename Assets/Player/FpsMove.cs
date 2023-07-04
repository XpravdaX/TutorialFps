// автор Pravda xd
using System.Collections;
using UnityEngine;

public class FpsMove : MonoBehaviour
{
    // Объявляем переменные внутри класса
    [Space(20)] public Rigidbody rb; // Переменная Rigidbody, на которую будет действовать физическая сила
    public float walkSpeed = 5f; // Скорость передвижения персонажа при ходьбе
    Vector3 playerInput; // Вектор для хранения управляющих нажатий
    Vector3 velocity; // Текущая скорость персонажа
    Vector3 velocityChange; // Разница между желаемой и текущей скоростью персонажа

    [Space(20)]
    public bool enableJump = true; // Переменная, разрешающая прыжки
    public float jumpPower = 5f; // Сила прыжка
    public KeyCode jumpKey = KeyCode.Space; // Кнопка прыжка
    public KeyCode runKey = KeyCode.LeftShift; // Кнопка бега

    private bool isJumping; // Флаг, что персонаж прыгает
    private bool isGrounded; // Флаг, что персонаж на земле

    // Обновление состояния объекта каждый кадр
    private void Update()
    {
        // Если разрешены прыжки, нажата кнопка прыжка и персонаж на земле, то выполняется прыжок
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // Проверяем, на земле ли персонаж
        CheckGround();
    }

    // Метод, вызывающийся каждый фиксированный кадр
    void FixedUpdate()
    {
        // Скорость передвижения персонажа равна скорости ходьбы
        float moveSpeed = walkSpeed;

        // Если нажата кнопка бега, то скорость удваивается
        if (Input.GetKey(runKey))
        {
            moveSpeed *= 2f;
        }

        // Получаем управляющие нажатия и преобразуем вектор в соответствии с ориентацией персонажа
        playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerInput = transform.TransformDirection(playerInput) * moveSpeed;

        // Получаем текущую скорость персонажа и вычисляем разницу
        velocity = rb.velocity;
        velocityChange = (playerInput - velocity);
        // При этом зануляем изменение по оси Y, чтобы персонаж не подпрыгивал вместе с движением
        velocityChange.y = 0;

        // Добавляем силу в соответствии с изменением скорости
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    // Проверяем землю и обновляем соответствующие флаги
    private void CheckGround()
    {
        // Определяем точку, в которой должна находиться нижняя точка персонажа 
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        // Определяем направление луча, который будет определять наличие земли
        Vector3 direction = transform.TransformDirection(Vector3.down);
        // Максимальная дистанция луча определяется константой 
        float distance = .75f;

        // Если луч пересекается с коллайдером земли, то персонаж на земле
        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.green); // Рисуем луч для отладки
            isGrounded = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
            isJumping = true;
        }
    }

    // Метод выполнения прыжка
    private void Jump()
    {
        if (isGrounded)
        {
            // Если персонаж на земле, то добавляем физическую силу для прыжка
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false; // Сбрасываем флаг нахождения на земле
        }
    }
}