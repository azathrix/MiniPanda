import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'MiniPanda',
  description: '轻量级脚本语言，专为 Unity 设计',
  lang: 'zh-CN',

  base: '/mini-panda/', // GitHub Pages 部署路径，根据实际仓库名修改

  head: [
    ['link', { rel: 'icon', href: '/favicon.ico' }]
  ],

  themeConfig: {
    logo: '/logo.png',

    nav: [
      { text: '首页', link: '/' },
      { text: '教程', link: '/tutorial/' },
      { text: 'API', link: '/api/' },
      { text: 'GitHub', link: 'https://github.com/your-username/mini-panda' }
    ],

    sidebar: {
      '/tutorial/': [
        {
          text: '教程',
          items: [
            { text: '快速入门', link: '/tutorial/' },
            { text: '基础语法', link: '/tutorial/basics' },
            { text: '控制流', link: '/tutorial/control-flow' },
            { text: '函数', link: '/tutorial/functions' },
            { text: '面向对象', link: '/tutorial/oop' },
            { text: '高级特性', link: '/tutorial/advanced' },
            { text: 'C# 互操作', link: '/tutorial/interop' },
            { text: '调试', link: '/tutorial/debugging' }
          ]
        }
      ],
      '/api/': [
        {
          text: 'API 参考',
          items: [
            { text: '概述', link: '/api/' },
            { text: 'MiniPanda 类', link: '/api/minipanda' },
            { text: 'Value 结构体', link: '/api/value' },
            { text: 'NativeFunction', link: '/api/native-function' },
            { text: '内置函数', link: '/api/builtins' },
            { text: '内置对象', link: '/api/objects' },
            { text: '语法参考', link: '/api/syntax' }
          ]
        }
      ]
    },

    socialLinks: [
      { icon: 'github', link: 'https://github.com/your-username/mini-panda' }
    ],

    footer: {
      message: 'Released under the MIT License.',
      copyright: 'Copyright © 2024 MiniPanda'
    },

    search: {
      provider: 'local'
    },

    outline: {
      level: [2, 3],
      label: '目录'
    },

    docFooter: {
      prev: '上一页',
      next: '下一页'
    },

    lastUpdated: {
      text: '最后更新'
    }
  }
})
