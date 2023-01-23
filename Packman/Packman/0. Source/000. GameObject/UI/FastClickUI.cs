﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packman
{
    internal class FastClickUI : GameObject
    {
        private string[] _text =
        {
            @"======================================================",
            @"|                                 _                  |",
            @"|                                | |                 |",
            @"| ___  _ __    __ _   ___   ___  | |__    __ _  _ __ |",
            @"|/ __|| '_ \  / _` | / __| / _ \ | '_ \  / _` || '__||",
            @"|\__ \| |_) || (_| || (__ |  __/ | |_) || (_| || |   |",
            @"||___/| .__/  \__,_| \___| \___| |_.__/  \__,_||_|   |",
            @"|     | |                                            |",
            @"|     |_|                                            |",
            @"======================================================",
        };

        // 프로그레스 바 관련 변수들..
        private string[] _progressBar = 
        {
            "=================================",
            "|                               |",
            "|                               |",
            "|                               |",
            "=================================",
        };

        // 프로그레바 게이지 시작 위치..
        private const int GAUGE_BAR_START_X = 1;
        private const int GAUGE_BAR_START_Y = 1;
        private const int GAUGE_BAR_END_X = 32;
        private const int GAUGE_BAR_WIDTH = GAUGE_BAR_END_X - GAUGE_BAR_START_X;

        private int _gaugeBarOffsetX = 10;
        private int _gaugeBarOffsetY = 5;

        private string[] _gaugeBar = null;

        float _curGauge = 0.0f;
        float _gaugePower = 0.01f;

        ConsoleColor[] _colors = { ConsoleColor.Gray, ConsoleColor.DarkGray };
        int _curColorIndex = 0;

        bool _isSpacebarKeyDown = false;
        bool _isCheckSpacebarKeyUp = false;

        public FastClickUI( int x, int y )
            : base( x, y, 2100 )
        {
            _gaugeBar = new string[GAUGE_BAR_WIDTH + 1];
            string image = "■";
            string accImage = "■";
            for ( int i = 1; i < _gaugeBar.Length; ++i )
            {
                _gaugeBar[i] = accImage;
                accImage = accImage + image;
            }
        }

        public void Initialize()
        {
            base.Initialize();

            OnUpdateColor();

            EventManager.Instance.AddInputEvent( ConsoleKey.Spacebar, OnPressSpacebarKey );
        }

        public override void OnEnable()
        {
            base.OnEnable();

            _curGauge = 0.0f;
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override void Update()
        {
            base.Update();

            if(false == _isCheckSpacebarKeyUp )
            {
                if ( _isSpacebarKeyDown )
                {
                    _curGauge = Math.Min( _curGauge + _gaugePower, 1.0f );
                    _isCheckSpacebarKeyUp = true;
                }
            }
            else
            {
                if ( false == _isSpacebarKeyDown )
                    _isCheckSpacebarKeyUp = false;
            }

            _isSpacebarKeyDown = false;
        }

        public override void Render()
        {
            base.Render();

            ConsoleColor tempColor = Console.ForegroundColor;

            int textLineCount = _text.Length;

            Console.ForegroundColor = _colors[_curColorIndex];
            for ( int i = 0; i <textLineCount; ++i )
            {
                Console.SetCursorPosition( _x, _y + i );
                Console.Write( _text[i] );
            }

            Console.ForegroundColor = tempColor;

            int gaugeBarIndex = (int)(GAUGE_BAR_WIDTH * (_curGauge + 0.0001f));

            int gaugeBarX = _x + _gaugeBarOffsetX;
            int gaugeBarY = Console.CursorTop + _gaugeBarOffsetY;

            for ( int i = 0; i < _progressBar.Length; ++i )
            {
                Console.SetCursorPosition( gaugeBarX, gaugeBarY + i );
                Console.Write( _progressBar[i] );
            }

            gaugeBarX += GAUGE_BAR_START_X;
            gaugeBarY = gaugeBarY + GAUGE_BAR_START_Y;

            for( int i = 0; i < 3; ++i)
            {
                Console.SetCursorPosition( gaugeBarX, gaugeBarY + i );
                Console.Write( _gaugeBar[gaugeBarIndex] );
            }
        }

        private void OnUpdateColor()
        {
            _curColorIndex = (_curColorIndex + 1) % _colors.Length;

            EventManager.Instance.SetTimeOut( OnUpdateColor, 0.1f );
        }

        private void OnPressSpacebarKey()
        {
            _isSpacebarKeyDown = true;
        }
    }
}