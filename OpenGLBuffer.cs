/*
  NoZ Game Engine

  Copyright(c) 2019 NoZ Games, LLC

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files(the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions :

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/

using System;
using System.Runtime.InteropServices;

namespace NoZ.Platform.OpenGL
{
    class OpenGLBuffer<T> : IDisposable
    {
        private const uint InvalidId = 0xFFFFFFFF;

        private T[] _data;
        private GCHandle _handle;
        private readonly int _stride;
        private int _activeRange;

        public uint Id { get; private set; } = InvalidId;
        public GL.BufferTarget Target { get; private set; }
        public int Count { get; private set; }
        public int Capacity => _data.Length;

        public ref T this[int index] => ref _data[index];

        public OpenGLBuffer(GL.BufferTarget target, int size)
        {
            _data = new T[size];
            Target = target;
            _handle = GCHandle.Alloc(_data, GCHandleType.Pinned);
            _stride = Marshal.SizeOf<T>();
            Count = 0;
        }

        /// <summary>
        /// Commit the buffer in its current state to the GPU
        /// </summary>
        public void Commit(uint activeBuffer)
        {
            var first = false;
            if (Id == InvalidId)
            {
                Id = GL.GenBuffer();
                first = true;

                if (typeof(T) == typeof(OpenGLVertex))
                {
                    GL.BindBuffer(Target, Id);
                    GL.EnableVertexAttribArray(0);
                    GL.EnableVertexAttribArray(1);
                    GL.EnableVertexAttribArray(2);
                    GL.VertexAttribPointer(0, 2, GL.VertexAttribPointerType.Float, true, OpenGLVertex.SizeInBytes, OpenGLVertex.OffsetXY);
                    GL.VertexAttribPointer(1, 4, GL.VertexAttribPointerType.UnsignedByte, true, OpenGLVertex.SizeInBytes, OpenGLVertex.OffsetColor);
                    GL.VertexAttribPointer(2, 2, GL.VertexAttribPointerType.Float, true, OpenGLVertex.SizeInBytes, OpenGLVertex.OffsetUV);
                }
            }

            if (activeBuffer != Id)
                GL.BindBuffer(Target, Id);

            if (Target == GL.BufferTarget.UniformBuffer)
            {
                var range = _stride * Count;
                if (_activeRange != range)
                {
                    _activeRange = range;
                    GL.BindBufferRange(Target, 0, Id, (IntPtr)0, (IntPtr)range);
                }
            }

#if false
            GL.BufferData (
                Target,
                _stride * Count,
                _handle.AddrOfPinnedObject(),
                GL.BufferUsage.StreamDraw
            );
#else
            if (first)
            {
                GL.BufferData(
                    Target,
                    _stride * Capacity,
                    _handle.AddrOfPinnedObject(),
                    GL.BufferUsage.StreamDraw
                );
            }
            else
            {
                GL.BufferSubData(
                    Target,
                    0,
                    _stride * Count,
                    _handle.AddrOfPinnedObject()
                );
            }
#endif

            Count = 0;
        }

        public void Add(in T value) => _data[Count++] = value;

        public void Add(T[] values, int offset, int count)
        {
            Array.Copy(values, offset, _data, Count, count);
            Count += count;
        }

        ~OpenGLBuffer()
        {
            if (_handle.IsAllocated)
                _handle.Free();
        }

        public IntPtr ToIntPtr() => _handle.AddrOfPinnedObject();

        public void Dispose()
        {
            if (_handle.IsAllocated)
                _handle.Free();
        }
    }
}
