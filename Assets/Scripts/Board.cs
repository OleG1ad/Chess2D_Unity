using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class Board : MonoBehaviour
{
    DragAndDrop dad = new DragAndDrop();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dad.Action();
    }

    class DragAndDrop
    {
        //для хранения состояния, в каком стостоянии находится интерфейс
        State state;
        // какой именно объект схватили
        GameObject item;
        // при взятии курсором смещение от центра фигуры к месту касания
        Vector2 offset;

        public DragAndDrop()
        {
            state = State.none;
            //никакой объект еще не схватили
            item = null;
        }

        public void Action()
        {
            switch (state)
            {
                case State.none:
                    // если есть действие (клик мышкой) то взять фигуру
                    if (IsMouseButtonPressed())
                        pickup();
                    break;

                case State.drag:
                    // если нажата кнопка
                    // то тащить фигуру
                    if (IsMouseButtonPressed())
                        drag();
                    // если был процес переноса и кнопка не нажата опустить фигуру
                    else
                        drop();
                    break;
                default:
                    break;
            }
        }

        bool IsMouseButtonPressed()
        {
            return Input.GetMouseButton(0);
        }

        Vector2 GetClickPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Transform GetItemAt(Vector2 position)
        {
            RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.1f);
            if (figures.Length == 0)
                return null;
            return figures[0].transform;
        }


        void pickup()
        {
            // проверяем что схватили
            Vector2 clickPosition = GetClickPosition();
            Transform clicedItem = GetItemAt(clickPosition);
            if (clicedItem == null)
                return;
            state = State.drag;
            // сохранили объект
            item = clicedItem.gameObject;
            // приведение позиции с 3D на 2D пространство и смещеие относительно центра 
            offset = (Vector2)clicedItem.position - clickPosition;
            Debug.Log("Picked" + item.name);
        }

        void drag()
        {
            // тащить фигуру
            // указываем объекту позицию мышки где она сейчас находится
            // добавляем к перемещению смещение
            item.transform.position = GetClickPosition() + offset;
        }

        void drop()
        {
            item = null;
            state = State.none;
        }

        // перечисление разных состояний фигуры
        enum State
        {
            //ничего не происходит
            none,
            // не успевает перейти в состояние схаватили фигуру
            //pick,
            //потащили фигуру
            drag
            // сбросили фигуру, не нужно стостояние т.к. сразу переходит в none
            //drop
        }
    }
}
