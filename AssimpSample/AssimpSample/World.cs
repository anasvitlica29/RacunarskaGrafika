// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using Assimp;
using System.IO;
using System.Reflection;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using SharpGL;

namespace AssimpSample
{


    /// <summary>
    ///  Klasa enkapsulira OpenGL kod i omogucava njegovo iscrtavanje i azuriranje.
    /// </summary>
    public class World : IDisposable
    {
        #region Atributi

        private int sirina;

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private AssimpScene m_scene;
        private AssimpScene m_scene2;

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        private float m_xRotation = 0.0f;

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        private float m_yRotation = 0.0f;

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        private float m_sceneDistance = 1000.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        #endregion Atributi

        #region Properties

        public int Sirina
        {
            get { return sirina; }
            set { sirina = value; }
        }

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
        }

        public AssimpScene Scene2
        {
            get { return m_scene2; }
            set { m_scene2 = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko X ose.
        /// </summary>
        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        /// <summary>
        ///	 Udaljenost scene od kamere.
        /// </summary>
        public float SceneDistance
        {
            get { return m_sceneDistance; }
            set { m_sceneDistance = value; }
        }

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, String sceneFileName2, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_scene2 = new AssimpScene(scenePath, sceneFileName2, gl);
            this.m_width = width;
            this.m_height = height;
        }

        /// <summary>
        ///  Destruktor klase World.
        /// </summary>
        ~World()
        {
            this.Dispose(false);
        }

        #endregion Konstruktori

        #region Metode

        /// <summary>
        ///  Korisnicka inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        public void Initialize(OpenGL gl)
        {
            // Crna pozadina i bela boja za crtanje
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 1f, 1f);

            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.FrontFace(OpenGL.GL_CCW);        //Lice je CCW, nalicje CW

            m_scene.LoadScene();    //Tabla
            m_scene.Initialize();
            m_scene2.LoadScene();   //Strelica
            m_scene2.Initialize();
        }

        /// <summary>
        ///  Iscrtavanje OpenGL kontrole.
        /// </summary>
        public void Draw(OpenGL gl)
        {
            // Ocisti sadrzaj kolor bafera i bafera dubine
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // Sacuvaj stanje ModelView matrice i primeni transformacije
            gl.PushMatrix();
            gl.Translate(0.0f, 0.0f, -m_sceneDistance);     //Udalji se od scene po z osi
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            #region Koordinatni pocetak
            gl.PushMatrix();
            gl.PointSize(5.0f);
            gl.Begin(OpenGL.GL_POINTS);
                gl.Color(0.0f, 1.0f, 0.0f);
                gl.Vertex(0.0f, 0.0f, 0.0f);
            gl.End();
            gl.PopMatrix();
            #endregion

            #region Pod
            gl.PushMatrix();
            gl.Translate(-60.0f, -60.0f, 0.0f);
            gl.Scale(15, 5, 15);
            gl.Begin(OpenGL.GL_QUADS);
                gl.Color(0.30f, 0.15f, 0.0f);
                gl.Vertex(0.0f, 0.0f, 0.0f);
                gl.Vertex(0.0f, 0.0f, 10.0f);
                gl.Vertex(10.0f, 0.0f, 10.0f);
                gl.Vertex(10.0f, 0.0f, 0.0f);
            gl.End();
            gl.PopMatrix();
            #endregion

            #region Zidovi
            gl.PushMatrix();
            gl.Translate(-60.0f, -10.0f, 0.0f);
            gl.Scale(15, 5, 15);
            gl.Rotate(90.0f, 0.0f, 0.0f);
            gl.Begin(OpenGL.GL_QUAD_STRIP);
            
                //Zadnji
                gl.Color(1.0f, 0.88f, 0.70f);
                gl.Vertex(0.0f, 0.0f, -20.0f);   //gornja leva
                gl.Vertex(0.0f, 0.0f, 10.0f);   //donja leva
                gl.Vertex(10.0f, 0.0f, -20.0f);
                gl.Vertex(10.0f, 0.0f, 10.0f);

                //Desni
                gl.Color(1.0f, 0.70f, 0.80f);
                gl.Vertex(10.0f, 10.0f, -20.0f);
                gl.Vertex(10.0f, 10.0f, 10.0f);

                //Prednji
                gl.Color(1.0f, 0.88f, 0.70f);
                gl.Vertex(0.0f, 10.0f, -20.0f);
                gl.Vertex(0.0f, 10.0f, 10.0f);

                //Levi
                gl.Color(1.0f, 0.70f, 0.80f);
                gl.Vertex(0.0f, 0.0f, -20.0f);  
                gl.Vertex(0.0f, 0.0f, 10.0f);   

            gl.End();
            gl.PopMatrix();
            #endregion

            #region Postolje
            gl.PushMatrix();
            gl.Color(0.30f, 0.15f, 0.0f);
            gl.Scale(20.0f, 30.0f, 2f);
            gl.Translate(0.0f, 0.5f, 1.0f);
            Cube postolje = new Cube();
            postolje.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
            #endregion

            #region Pikado tabla
            gl.PushMatrix();
            gl.Translate(0.0f, 0.0f, 5.0f);
            m_scene.Draw();
            gl.PopMatrix();
            #endregion

            #region Strelice
            gl.PushMatrix();
            gl.Translate(0.0f, 5.0f, 5.0f);
            m_scene2.Draw();

            gl.Translate(5.0f, 0.0f, 0.0f);
            m_scene2.Draw();

            gl.Translate(5.0f, 0.0f, 0.0f);
            m_scene2.Draw();
            gl.PopMatrix();
            #endregion

            #region Slova
            gl.PushMatrix();
            gl.Viewport(m_width / 2, 0, m_width / 2, m_height / 2);     //donji desni ugao
            gl.DrawText3D("Arial Bold", 14, 0, 0, "");
            gl.DrawText(sirina - 220, 100, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Predmet: Racunarska grafika");
            gl.DrawText(sirina - 220, 80, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Sk.god: 2019/20.");
            gl.DrawText(sirina - 220, 60, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Ime: Ana");
            gl.DrawText(sirina - 220, 40, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Prezime: Svitlica");
            gl.DrawText(sirina - 220, 20, 1.0f, 0.0f, 0.0f, "Arial Bold", 14, "Sifra zad: 3.2");
            gl.PopMatrix();
            #endregion

            gl.PopMatrix();
            // Oznaci kraj iscrtavanja
            gl.Flush();
        }


        /// <summary>
        /// Podesava viewport i projekciju za OpenGL kontrolu.
        /// </summary>
        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;
            gl.Viewport(0, 0, m_width, m_height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);      // selektuj Projection Matrix
            gl.LoadIdentity();
            gl.Perspective(50f, (double)width / height, 1.0f, 20000f);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();                // resetuj ModelView Matrix
        }

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_scene.Dispose();
            }
        }

        #endregion Metode

        #region IDisposable metode

        /// <summary>
        ///  Dispose metoda.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
