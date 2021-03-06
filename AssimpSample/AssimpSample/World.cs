﻿// -----------------------------------------------------------------------
// <file>World.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Srđan Mihić</author>
// <author>Aleksandar Josić</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod.</summary>
// -----------------------------------------------------------------------
using System;
using SharpGL.SceneGraph.Primitives;
using SharpGL;
using System.Drawing;
using System.Drawing.Imaging;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.SceneGraph.Core;
using System.Windows.Threading;

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

        // TEKSTURE
        private uint[] m_textures;
        private string[] m_textureFiles = { "..//..//Images//bricks.jpg", "..//..//Images//whitewood.jpg", "..//..//Images//woodfloor.jpg" };
        private enum TextureObjects { Brick = 0, WhiteWood, Floor };
        private readonly int m_textureCount = Enum.GetNames(typeof(TextureObjects)).Length;

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
        private float m_sceneDistance = 100.0f;

        /// <summary>
        ///	 Sirina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_width;

        /// <summary>
        ///	 Visina OpenGL kontrole u pikselima.
        /// </summary>
        private int m_height;

        //Za sijalicu
        /// <summary>
        ///	 Pozicija reflektorskog svetlosnog izvora.
        /// </summary>
        private float[] m_spotPosition = { 0.0f, 90.0f, 90.0f };
        private float[] m_spotPosition1 = { 0.0f, 60.0f, 5.0f };

        /// <summary>
        ///	 Promenljive za iscrtavanje lampe
        /// </summary>
        private Sphere sphereLamp;

        private Sphere sijalica;

        //Stavka 7
        private float translateY = 0.0f;
        private float translateX = 0.0f;
        private double scaleDarts = 1;
        private float reflectorAmbientRed = 1.0f;
        private float reflectorAmbientGreen = 0.0f;
        private float reflectorAmbientBlue = 0.0f;

        //Stavka 11: Animacija
        private bool animationInProgress = false;
        private DispatcherTimer timer1;
        private DispatcherTimer timer2;
        private DispatcherTimer timer3;
        private DispatcherTimer timer4;
        private DispatcherTimer timer5;

        private float m_throwDart1_x = 0.0f;
        private float m_throwDart1_y = 0.0f;
        private float m_throwDart1_z = 100.0f;

        private float m_throwDart2_x = 0.0f;
        private float m_throwDart2_y = 0.0f;
        private float m_throwDart2_z = 100.0f;

        private float m_throwDart3_x = 0.0f;
        private float m_throwDart3_y = 0.0f;
        private float m_throwDart3_z = 100.0f;

        private double m_dartboardScale = 1;

        #endregion Atributi

        #region Properties

       
        public float ReflectorAmbientRed
        {
            get
            {
                return this.reflectorAmbientRed;
            }
            set
            {
                this.reflectorAmbientRed = value;
            }
        }

        public float ReflectorAmbientGreen {
            get
            {
                return this.reflectorAmbientGreen;
            }
            set
            {
                this.reflectorAmbientGreen = value;
            }
        }

        public float ReflectorAmbientBlue
        {
            get
            {
                return this.reflectorAmbientBlue;
            }
            set
            {
                this.reflectorAmbientBlue = value;
            }
        }

        public double ScaleDarts
        {
            get { return this.scaleDarts; }
            set { this.scaleDarts = value; }
        }

        public float TranslateY
        {
            get
            {
                return this.translateY;
            }
            set
            {
                this.translateY = value;
            }
        }

        public float TranslateX
        {
            get
            {
                return this.translateX;
            }
            set
            {
                this.translateX = value;
            }
        }

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
            set {
                m_xRotation = value;
                if (m_xRotation >= 90)
                {
                    m_xRotation = 90;
                }
                if (m_xRotation <= 0)
                {
                    m_xRotation = 0;
                }
            }
        }

        /// <summary>
        ///	 Ugao rotacije sveta oko Y ose.
        /// </summary>
        public float RotationY
        {
            get { return m_yRotation; }
            set {
                m_yRotation = value;
                //if (m_yRotation )
            }
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

        public bool AnimationInProgress
        {
            get
            {
                return this.animationInProgress;
            }
            set
            {
                this.animationInProgress = value;
            }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase World.
        /// </summary>
        public World(String scenePath, String sceneFileName, String sceneFileName2, int width, int height, OpenGL gl)
        {
            m_textures = new uint[m_textureCount];
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
            // Stavka 1
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE);

            // Stavka 2 - Tackasti izvor zute boje
            SetupLighting(gl);
            
            sphereLamp = new Sphere();
            sphereLamp.CreateInContext(gl);
            sphereLamp.Radius = 3f;
            sphereLamp.Material = new SharpGL.SceneGraph.Assets.Material();
            //sphereLamp.Material.Emission = Color.LightYellow;

            sijalica = new Sphere();
            sijalica.CreateInContext(gl);
            sijalica.Radius = 2f;
            sijalica.Material = new SharpGL.SceneGraph.Assets.Material();
            //sijalica.Material.Emission = Color.Red;

            // Crna pozadina i bela boja za crtanje
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Color(1f, 1f, 1f);

            // Model sencenja na flat (konstantno)
            gl.ShadeModel(OpenGL.GL_FLAT);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.FrontFace(OpenGL.GL_CCW);

            
            // Stavka 3 - Teksture
            // Teksture se primenjuju sa parametrom decal
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL);

            //Ucitaj slike i kreiraj teksture
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);  // Linear mipmap Filtering
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR_MIPMAP_LINEAR);  // Linear mipmap Filtering
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
            
            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(m_textureFiles[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                      System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, imageData.Scan0);
                
                image.UnlockBits(imageData);
                image.Dispose();
            }

            gl.Disable(OpenGL.GL_TEXTURE_2D);

            m_scene.LoadScene();    //Tabla
            m_scene.Initialize();
            m_scene2.LoadScene();   //Strelica
            m_scene2.Initialize();
        }

        private void SetupLighting(OpenGL gl)
        {
            //Tackasti izvor svetlozute boje na centru plafona
            float[] light0pos = new float[] { 15.0f, 85.0f, 75.0f, 1.0f };
            float[] light0ambient = new float[] { 0.4f, 0.3f, 0.0f, 1f };
            float[] light0diffuse = new float[] { 0.7f, 0.6f, 0.0f, 1.0f };
            float[] light0specular = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f);      //Tackasto
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0specular);

            //Reflektor
            float[] light1pos = new float[] { m_spotPosition1[0], m_spotPosition1[1], m_spotPosition1[2], 1.0f };
            float[] light1ambient = new float[] { 1.0f, 0.0f, 0.0f, 1.0f };
            float[] light1diffuse = new float[] { 1.0f, 0.0f, 0.0f, 1.0f };
            float[] smer = new float[] { 0.0f, 1.0f, 0.0f };       //gleda na dole
            
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, light1ambient);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 30.0f);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, smer);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, light0specular);

            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);
            gl.Enable(OpenGL.GL_LIGHT1);

            // Ukljuci automatsku normalizaciju nad normalama
            gl.Enable(OpenGL.GL_NORMALIZE);
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
            gl.Translate(this.translateX, this.translateY, -m_sceneDistance);     //Udalji se od scene po z osi
            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            gl.LookAt( 0, 0, m_sceneDistance, 
                       0, 0, 0, 
                       0, 1, 0);

            #region Tackasto osvetljenje
            gl.PushMatrix();
            float[] pos = new float[] { 0.0f, 90.0f, 90.0f, 1.0f };
            gl.Color(0.4f, 0.3f, 0.0f);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, pos);
            gl.Translate(pos[0], pos[1], pos[2]);
            sphereLamp.Material.Bind(gl);
            sphereLamp.Render(gl, RenderMode.Render);
            gl.PopMatrix();
            #endregion

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
            gl.Enable(OpenGL.GL_TEXTURE_2D);
           
            //Stavka 5
            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Floor]);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.LoadIdentity();
            gl.Scale(5, 5, 5);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            gl.Begin(OpenGL.GL_QUADS);
                gl.Color(1.0f, 1.0f, 1.0f);
                gl.Normal(0f, 1f, 0f);
                gl.TexCoord(1f, 1f);
                gl.Vertex(-60.0f, -60.0f, 0.0f);
                gl.TexCoord(0f, 1f);
                gl.Vertex(-60.0f, -60.0f, 150.0f);
                gl.TexCoord(0f, 0f);
                gl.Vertex(90.0f, -60.0f, 150.0f);
                gl.TexCoord(1f, 0f);
                gl.Vertex(90.0f, -60.0f, 0.0f);
            gl.End();

            gl.MatrixMode(OpenGL.GL_TEXTURE);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.LoadIdentity();
            gl.Scale(1, 1, 1);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();
            #endregion

            #region Zidovi
            gl.PushMatrix();
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            //for (float z = 60.0f; z >= 0.0f; z -= 10.0f)
            //{
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Brick]);

                gl.Begin(OpenGL.GL_QUADS);
                //Zadnji
                gl.Normal(0f, 0f, 1f);
                gl.TexCoord(0.0f, 1.0f);
                gl.Vertex(-60.0f, 90.0f, 0.0f);   //gornja leva
                gl.TexCoord(0.0f, 0.0f);
                gl.Vertex(-60.0f, -60.0f, 0.0f);   //donja leva
                gl.TexCoord(1.0f, 0.0f);
                gl.Vertex(90.0f, -60.0f, 0.0f);     //donja desna
                gl.TexCoord(1.0f, 1.0f);
                gl.Vertex(90.0f, 90.0f, 0.0f);      //gornja desna
                gl.End();

                gl.Begin(OpenGL.GL_QUADS);
                //Desni
                gl.Normal(-1f, 0f, 0f);
                gl.TexCoord(1.0f, 1.0f);
                gl.Vertex(90.0f, 90.0f, 0.0f);      //gornja leva
                gl.TexCoord(1.0f, 0.0f);
                gl.Vertex(90.0f, -60.0f, 0.0f);     //donja leva
                gl.TexCoord(0.0f, 0.0f);
                gl.Vertex(90.0f, -60.0f, 150.0f);   //donja desna
                gl.TexCoord(0.0f, 1.0f);
                gl.Vertex(90.0f, 90.0f, 150.0f);    //gornja desna
                gl.End();

                gl.Begin(OpenGL.GL_QUADS);
                //Prednji
                gl.Normal(0f, 0f, -1f);
                gl.TexCoord(0.0f, 1.0f);
                gl.Vertex(90.0f, 90.0f, 150.0f);    //gornja leva
                gl.TexCoord(0.0f, 0.0f);
                gl.Vertex(90.0f, -60.0f, 150.0f);   //donja leva
                gl.TexCoord(1.0f, 0.0f);
                gl.Vertex(-60.0f, -60.0f, 150.0f);  // donja desna
                gl.TexCoord(1.0f, 1.0f);
                gl.Vertex(-60.0f, 90.0f, 150.0f);   //gornja desna
                gl.End();

                gl.Begin(OpenGL.GL_QUADS);
                //Levi
                gl.Normal(1f, 0f, 0f);
                gl.TexCoord(1.0f, 1.0f);
                gl.Vertex(-60.0f, 90.0f, 150.0f);   //gornja leva
                gl.TexCoord(1.0f, 0.0f);
                gl.Vertex(-60.0f, -60.0f, 150.0f);  // donja leva
                gl.TexCoord(0.0f, 0.0f);
                gl.Vertex(-60.0f, -60.0f, 0.0f);
                gl.TexCoord(0.0f, 1.0f);
                gl.Vertex(-60.0f, 90.0f, 0.0f);
                gl.End();
            //}
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();
            #endregion

            #region Plafon
            gl.PushMatrix();
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(-60.0f, 90.0f, 150.0f);
            gl.Vertex(-60.0f, 90.0f, 0.0f);
            gl.Vertex(90.0f, 90.0f, 0.0f);
            gl.Vertex(90.0f, 90.0f, 150.0f);
            gl.End();
            gl.PopMatrix();
            #endregion

            #region Postolje
            gl.PushMatrix();
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)TextureObjects.Floor]);
            gl.Scale(20.0f, 30.0f, 2f);
            gl.Translate(0.0f, 0.0f, 1.0f);
            Cube postolje = new Cube();
            postolje.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();
            #endregion

            #region Pikado tabla
            gl.PushMatrix();
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.Scale(0.5f * this.m_dartboardScale, 0.6f * this.m_dartboardScale, 1.0f);
            //gl.Translate(0.0f, 15.0f, 4.0f);
            gl.Translate(0.0f, 0.0f, 4.0f);
            m_scene.Draw();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();
            #endregion

            #region Strelice
            //Prva
            gl.PushMatrix();
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.Scale(scaleDarts, scaleDarts, scaleDarts);
            gl.Translate(0.0f + this.m_throwDart1_x, 5.0f + this.m_throwDart1_y, 5.0f + this.m_throwDart1_z);
            m_scene2.Draw();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();

            //Druga
            gl.PushMatrix();
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.Scale(scaleDarts, scaleDarts, scaleDarts);
            gl.Translate(0.0f + this.m_throwDart2_x, 10.0f + this.m_throwDart2_y, 5.0f + this.m_throwDart2_z);
            m_scene2.Draw();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PopMatrix();

            //Treca
            gl.PushMatrix();
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);
            gl.Scale(scaleDarts, scaleDarts, scaleDarts);
            gl.Translate(0.0f, 0.0f, 5.0f + this.m_throwDart3_z);
            m_scene2.Draw();
            gl.Disable(OpenGL.GL_TEXTURE_2D);
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

            #region Reflektor
            float[] light1pos = new float[] { m_spotPosition1[0], m_spotPosition1[1], m_spotPosition1[2], 1.0f };
            float[] light1diffuse = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] light1ambient = new float[] { ReflectorAmbientRed, ReflectorAmbientGreen, ReflectorAmbientBlue, 1.0f };

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, light1pos);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, light1ambient);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, light1diffuse);
            
            gl.Translate(m_spotPosition1[0], m_spotPosition1[1], m_spotPosition1[2]);
            //sijalica.Material.Emission = Color.Red;
            sijalica.Material.Bind(gl);
            sijalica.Render(gl, RenderMode.Render);
            gl.PopMatrix();
            #endregion

            gl.PopMatrix();
            // Oznaci kraj iscrtavanja
            gl.Flush();
        }

        #region Animacija
        public void Animation()
        {
            this.animationInProgress = true;
            timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromMilliseconds(250);
            timer1.Tick += new EventHandler(ThrowDarts1);
            timer1.Start();
        }

        public void ThrowDarts1(object sender, EventArgs e)
        {
            if (m_throwDart1_z > 0)         //priblizava se tabli
            {
                m_throwDart1_z -= 10f;
                if (m_throwDart1_x > -15.0)
                {
                    m_throwDart1_x -= 1.5f;
                    if(m_throwDart1_y > -20.0 )
                    {
                        m_throwDart1_y -= 1.0f;
                    }
                }
            }
            else
            {
                Animation2();
                timer1.Stop();

            }
        }

        public void Animation2()
        {
            timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromMilliseconds(250);
            timer2.Tick += new EventHandler(ThrowDarts2);
            timer2.Start();
        }

        public void ThrowDarts2(object sender, EventArgs e)
        {
            if (m_throwDart2_z > 0)         //priblizava se tabli
            {
                m_throwDart2_z -= 10f;
                if (m_throwDart2_x < 15.0)
                {
                    m_throwDart2_x += 1.5f;
                    if (m_throwDart2_y > -20.0)
                    {
                        m_throwDart2_y -= 1.0f;
                    }
                }
            }
            else
            {
                Animation3();
                timer2.Stop();

            }
        }

        public void Animation3()
        {
            timer3 = new DispatcherTimer();
            timer3.Interval = TimeSpan.FromMilliseconds(250);
            timer3.Tick += new EventHandler(ScaleDartboard);
            timer3.Start();
        }

        public void ScaleDartboard(object sender, EventArgs e)
        {
            if (m_dartboardScale < 3)     
            {
                m_dartboardScale += 1;
            }
            else
            {
                Animation4();
                timer3.Stop();

            }
        }

        public void Animation4()
        {
            timer4 = new DispatcherTimer();
            timer4.Interval = TimeSpan.FromMilliseconds(250);
            timer4.Tick += new EventHandler(ThrowDarts3);
            timer4.Start();
        }

        public void ThrowDarts3(object sender, EventArgs e)
        {
            if (m_throwDart3_z > 0)         //priblizava se tabli
            {
                m_throwDart3_z -= 10f;
                if (m_throwDart3_y > -100.0)
                {
                    m_throwDart3_y -= 10.0f;
                }
            }
            else
            {
                Animation5();
                timer4.Stop();

            }
        }

        public void Animation5()
        {
            timer5 = new DispatcherTimer();
            timer5.Interval = TimeSpan.FromMilliseconds(250);
            timer5.Tick += new EventHandler(ScaleDartboard1);
            timer5.Start();
        }

        public void ScaleDartboard1(object sender, EventArgs e)
        {
            if (m_dartboardScale > 1)
            {
                m_dartboardScale -= 1;
            }
            else
            {
                animationInProgress = false;
                timer5.Stop();

            }
        }

        public void Reset()
        {
            AnimationInProgress = false;
            m_throwDart1_x = 0.0f;
            m_throwDart1_y = 0.0f;
            m_throwDart1_z = 100.0f;
            m_throwDart2_x = 0.0f;
            m_throwDart2_y = 0.0f;
            m_throwDart2_z = 100.0f;
            m_throwDart3_x = 0.0f;
            m_throwDart3_y = 0.0f;
            m_throwDart3_z = 100.0f;
            m_dartboardScale = 1;

    }
        #endregion

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
