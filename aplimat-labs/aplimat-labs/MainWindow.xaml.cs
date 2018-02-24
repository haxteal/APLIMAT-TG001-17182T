using aplimat_labs.Models;
using aplimat_labs.Utilities;
using SharpGL;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aplimat_labs
{
    public partial class MainWindow : Window
    {

        private const float LINE_SMOOTHNESS = 0.02f;
        private const float GRAPH_LIMIT = 15;
        private const int TOTAL_CIRCLE_ANGLE = 360;

        //private Vector3 a = new Vector3(15, 15, 0);
        //private Vector3 b = new Vector3(-2, 10, 0); //0 z, y, z

        private const int HEADS = 0;
        private const int TAILS = 1;

        private const int UP = 0;
        private const int DOWN = 1;
        private const int LEFT = 2;
        private const int RIGHT = 3;
        private const int UP_LEFT = 4;
        private const int UP_RIGHT = 5;
        private const int DOWN_LEFT = 6;
        private const int DOWN_RIGHT = 7;

        private Randomizer rngDirection = new Randomizer(UP, DOWN_RIGHT);

        private Randomizer rng = new Randomizer(HEADS, TAILS);

        private Randomizer yPos = new Randomizer(-20, 20);

        private Randomizer colorOn = new Randomizer(0, 1);

        private Randomizer cubeSize = new Randomizer(1, 50);


        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            //Vector3 c = a + b;
            //Console.WriteLine("Vector c values: x: " + c.x + " y: " + c.y + " z: " + c.z);

            //Vector3 d = a - b;
            //Console.WriteLine("Vector c values: x: " + d.x + " y: " + d.y + " z: " + d.z);

            
        }

        //private List<CubeMesh> myCubes = new List<CubeMesh>();
        //private CubeMesh myCube = new CubeMesh(2, 1, 0);

        int cnt = 0;

        private Vector3 mousePos = new Vector3(0, 0, 0);

        //private CubeMesh myCube = new CubeMesh();
        //private Vector3 velocity = new Vector3(1, 1, 0);
        //private float speed = 2.0f;

        //private Vector3 myVector = new Vector3();
        //private Vector3 a = new Vector3(3, 5, 0);
        //private Vector3 b = new Vector3(-7, -6, 0);


        //private Vector3 origin = new Vector3(0, 0, 0);
        //private Vector3 tip = new Vector3(5, 7, 0);
        //private Vector3 magnitude = new Vector3();

        //private CubeMesh mover = new CubeMesh(-25, 0, 0);
        //private Vector3 acceleration = new Vector3(0.01f, 0, 0);

        private CubeMesh lightCube = new CubeMesh()
        {
            //Acceleration = new Vector3(0.1f, 0, 0),
            Position = new Vector3(-25, 0, 0)
        };

        private CubeMesh midCube = new CubeMesh()
        {
            //Acceleration = new Vector3(0.1f, 0, 0),
            Position = new Vector3(-15, 0, 0),
            //Position = new Vector3(-25, -10, 0),
            Mass = 3f
        };

        private CubeMesh heavyCube = new CubeMesh()
        {
            //Acceleration = new Vector3(0.1f, 0, 0),
            Position = new Vector3(-10, 0, 0),
            //Position = new Vector3(-25, -10, 0),
            Mass = 5f
        };

        private Vector3 wind = new Vector3(0.1f, 0, 0);
        private Vector3 gravity = new Vector3(0, -0.098f, 0);

        float boxT = 10.0f;
        float boxB = -10.0f;
        float boxL = -25.0f;
        float boxR = 25.0f;

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            

            OpenGL gl = args.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            
            gl.Translate(0.0f, 0.0f, -40.0f);

            gl.Color(1.0f, 0, 0);
            lightCube.Draw(gl);
            bounce(lightCube);

            gl.Color(0, 1.0f, 0);
            midCube.Draw(gl);
            bounce(midCube);

            gl.Color(0, 0, 1.0f);
            heavyCube.Draw(gl);
            bounce(heavyCube);


            gl.DrawText(20, 20, 1, 0, 0, "Arial", 25, lightCube.Velocity.y + " ");
            gl.DrawText(20, 45, 0, 1, 0, "Arial", 25, lightCube.Acceleration.y + " ");
            gl.DrawText(20, 70, 0, 0, 1, "Arial", 25, lightCube.Position.y + " ");
            #region meh

            //feb17===========================================================================
            //mover.Draw(gl);
            //mover.Velocity.Clamp(2.0f);
            //mover.Velocity.ClampMin(-2.0f);

            //if (cnt == 0)
            //{
            //    mover.Velocity += acceleration;
            //}
            //if (cnt == 1)
            //{
            //    mover.Velocity -= acceleration;
            //}

            //if (mover.Position.x >= 25.0f)
            //{
            //    cnt = 1;
            //    //mover.Position.x = -25.0f;
            //    mover.Velocity.x = -mover.Velocity.x;
            //    acceleration.x = 0.05f;
            //}

            //if (mover.Velocity.x <= -2.0f)
            //{
            //    mover.Velocity.x = 0.0f;
            //    acceleration.x = -0.0f;
            //}




            //gl.DrawText(20, 20, 1, 0, 0, "Arial", 25, mover.Velocity.x + " ");
            //feb17===========================================================================

















            //mousePos.Normalize();
            //mousePos *= 10;

            //gl.LineWidth(30.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(0, 0, 0);
            //gl.Vertex(mousePos.x, mousePos.y, 0);
            //gl.End();

            //gl.LineWidth(3.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Color(1.0f, 1.0f, 1.0f);
            //gl.Vertex(0, 0, 0);
            //gl.Vertex(mousePos.x, mousePos.y, 0);
            //gl.End();

            //gl.LineWidth(30.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Color(1.0f, 0.0f, 0.0f);
            ////if (cnt <= 10)
            ////{
            ////    gl.Vertex(10, 0, 0);
            ////}
            ////if (cnt > 10)
            ////{
            ////    gl.Vertex(-10, 0, 0);
            ////}
            ////if (cnt == 20)
            ////    cnt = 0;
            //gl.Vertex(10, 0, 0);
            //gl.Vertex(mousePos.x, mousePos.y, 0);
            //gl.End();

            //gl.LineWidth(30.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(-10, 0, 0);
            //gl.Vertex(mousePos.x, mousePos.y, 0);
            //gl.End();

            //gl.LineWidth(30.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Vertex(0, 10, 0);
            //gl.Vertex(mousePos.x, mousePos.y, 0);
            //gl.End();




            //quiz#01===========================================================
            //Console.WriteLine(cnt);

            //CubeMesh myCube = new CubeMesh();
            //myCube.Position = new Vector3(Gaussian.Generate(0, 15), yPos.GenerateInt(), 0);
            //myCubes.Add(myCube);

            //foreach (var cube in myCubes)
            //{
            //    gl.Color((float)colorOn.GenerateInt(), (float)colorOn.GenerateInt(), (float)colorOn.GenerateInt());
            //    cube.Draw(gl);
            //}

            ////clear on frame 100
            //if (cnt == 100)
            //{
            //    cnt = 0;
            //    myCubes.Clear();
            //}



            //quiz#01End===========================================================


            //Feb10================================================================
            //Seatwork #2==========================================================
            //myCube.Draw(gl);
            //myCube.Position += velocity * speed;

            //if (myCube.Position.x >= 25.0f)
            //{
            //    velocity.x = -1;
            //}

            //if (myCube.Position.y >= 15.0f)
            //{
            //    velocity.y = -1;
            //}

            //if (myCube.Position.x <= -25.0f)
            //{
            //    velocity.x = 1;
            //}

            //if (myCube.Position.y <= -15.0f)
            //{
            //    velocity.y = 1;
            //}
            //SW#2 End ===========================================================

            //myVector = a - b;
            ////vector a
            //gl.Color(1.0f, 0.0f, 0.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Vertex(0, 0);
            //gl.Vertex(a.x, a.y);
            //gl.End();

            ////vector b
            //gl.Color(0.0f, 1.0f, 0.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Vertex(a.x, a.y);
            //gl.Vertex(b.x, b.y);
            //gl.End();

            //gl.Color(0.0f, 0.0f, 1.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);

            //gl.Vertex(b.x, b.y);
            //gl.Vertex(0, 0);
            //gl.End();


            //Seatwork #3 =========================================================

            //gl.LineWidth(1.0f);
            //gl.Color(0.0f, 1.0f, 0.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Vertex(origin.x, origin.y);
            //gl.Vertex(tip.x, tip.y);
            //gl.End();

            //gl.LineWidth(5.0f);
            //gl.Color(0.0f, 0.0f, 1.0f);
            //gl.Begin(OpenGL.GL_LINE_STRIP);
            //gl.Vertex(origin.x, origin.y);
            //gl.Vertex(tip.x, tip.y);
            //gl.End();


            //magnitude = tip - origin;
            //Console.WriteLine(magnitude.GetMagnitude());

            //gl.DrawText(10, 10, 1.0f, 1.0f, 1.0f, "Arial", 15.0f, "Lenght: " + magnitude.GetMagnitude());
            //Feb10================================================================






            //myCube.Draw(gl);


            //myCube.Position += new Vector3(0, 1, 0);

            //switch(rngDirection.GenerateInt())
            //{
            //    case UP:
            //        myCube.Position += new Vector3(0, 0.1f, 0);
            //        break;
            //    case DOWN:
            //        myCube.Position += new Vector3(0, -0.1f, 0);
            //        break;
            //    case LEFT:
            //        myCube.Position += new Vector3(-0.1f, 0, 0);
            //        break;
            //    case RIGHT:
            //        myCube.Position += new Vector3(0.1f, 0, 0);
            //        break;
            //    case UP_LEFT:
            //        myCube.Position += new Vector3(-0.1f, 0.1f, 0);
            //        break;
            //    case UP_RIGHT:
            //        myCube.Position += new Vector3(0.1f, 0.1f, 0);
            //        break;
            //    case DOWN_LEFT:
            //        myCube.Position += new Vector3(-0.1f, -0.1f, 0);
            //        break;
            //    case DOWN_RIGHT:
            //        myCube.Position += new Vector3(0.1f, -0.1f, 0);
            //        break;
            //}




            ////gl.Color(0, 1, 0);
            //DrawCartesianPlane(gl); //draw cartesian plane with unit lines
            //DrawPoint(gl, 1, 1); //draw a point with coordinates (1, 1)
            //DrawLinearFunction(gl);
            //DrawQuadraticFunction(gl);
            //DrawCircle(gl);

            #endregion

        }

        private void bounce (CubeMesh cube)
        {
                cube.ApplyForce(wind);
                cube.ApplyForce(gravity);

            if (cube.Position.y <= boxB)
            {
                cube.Velocity.y -= cube.Velocity.y * 2;
            }

            if (cube.Position.y >= boxT)
            {
                cube.Velocity.y -= cube.Velocity.y * 2;
            }
            if (cube.Position.x >= boxR)
            {
                cube.Velocity.x -= cube.Velocity.x * 2;
            }
            if (cube.Position.x <= boxL)
            {
                cube.Velocity.x -= cube.Velocity.x * 2;
            }
        }


        private void DrawCartesianPlane(OpenGL gl)
        {

            //draw y-axis
            gl.Begin(OpenGL.GL_LINE_STRIP);

            gl.Color(1.0f, 0.0f, 1.0f);
            gl.Vertex(0, -GRAPH_LIMIT, 0);
            gl.Vertex(0, GRAPH_LIMIT, 0);
            gl.End();

            //draw x-axis
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Vertex(-GRAPH_LIMIT, 0, 0);
            gl.Vertex(GRAPH_LIMIT, 0, 0);
            gl.End();

            //draw unit lines
            for (int i = -15; i <= 15; i++)
            {
                gl.Begin(OpenGL.GL_LINE_STRIP);
                gl.Vertex(-0.2f, i, 0);
                gl.Vertex(0.2f, i, 0);
                gl.End();

                gl.Begin(OpenGL.GL_LINE_STRIP);
                gl.Vertex(i, -0.2f, 0);
                gl.Vertex(i, 0.2f, 0);
                gl.End();
            }
        }

        private void DrawPoint(OpenGL gl, float x, float y)
        {
            gl.PointSize(5.0f);
            gl.Begin(OpenGL.GL_POINTS);
            gl.Vertex(x, y);
            gl.End();
        }

        private void DrawLinearFunction(OpenGL gl)
        {
            /*
             * f(x) = x + 2;
             * Let x be 4, then y = 6 (4, 6)
             * Let x be -5, then y = -3 (-5, -3)
             * */
            gl.PointSize(2.0f);
            gl.Begin(OpenGL.GL_POINTS);
            for (float x = -(GRAPH_LIMIT - 5); x <= (GRAPH_LIMIT - 5); x+=LINE_SMOOTHNESS)
            {
                gl.Vertex(x, x + 2);
            }
            gl.End();

            DrawText(gl, "f(x) = x + 2", 500, 400);

        }


        private void DrawQuadraticFunction(OpenGL gl)
        {
            /*
             * f(x) = x^2 + 2x - 5;
             * Let x be 2, then y = 3
             * Let x be -1, then y = -6
             */

            //gl.PointSize(1.0f);
            //gl.Begin(OpenGL.GL_POINTS);
            //for (float x = -(GRAPH_LIMIT - 5); x <= (GRAPH_LIMIT - 5); x += LINE_SMOOTHNESS)
            //{
            //    gl.Vertex(x, Math.Pow(x, 2) + (2 * x) - 5);
            //}
            //gl.End();

            /*
             * f(x) = x^2
             * 
             */
            gl.PointSize(2.0f);
            gl.Begin(OpenGL.GL_POINTS);
            for (float x = -(GRAPH_LIMIT - 5); x <= (GRAPH_LIMIT - 5); x += LINE_SMOOTHNESS)
            {
                gl.Vertex(x, Math.Pow(x, 2));
            }
            gl.End();

            DrawText(gl, "f(x) = x ^ 2", 360, 380);

        }

        private void DrawCircle(OpenGL gl)
        {
            float radius = 3.0f;

            gl.PointSize(2.0f);
            gl.Begin(OpenGL.GL_POINTS);
            for (int i = 0; i <= TOTAL_CIRCLE_ANGLE; i++)
            {
                gl.Vertex(Math.Cos(i) * radius, Math.Sin(i) * radius);
            }
            gl.End();

            DrawText(gl, "(cos(x), sin(x))", 350, 200);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    break;
            }
        } 
        #region opengl init
        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            float[] global_ambient = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
            float[] light0pos = new float[] { 0.0f, 5.0f, 10.0f, 1.0f };
            float[] light0ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] light0diffuse = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] light0specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };

            float[] lmodel_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0specular);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_LIGHT0);

            

            gl.ShadeModel(OpenGL.GL_SMOOTH);
            


        }
        #endregion

        #region draw text
        private void DrawText(OpenGL gl, string text, int x, int y)
        {
            gl.DrawText(x, y, 1, 1, 1, "Arial", 12, text);
        }



        #endregion

        private void OpenGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            mousePos.x = (float)pos.X - (float)Width / 2.0f;
            mousePos.y = (float)pos.Y - (float)Height / 2.0f;

            mousePos.y = -mousePos.y;

            Console.WriteLine("mouse x:" + mousePos.x + " y:" + mousePos.y);
        }
    }
}
