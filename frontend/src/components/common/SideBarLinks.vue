<script setup>
const pages = [
    { name: 'Главная', path: '/' },
    { name: 'Отправить', path: '/send' },
]
</script>

<template>
   <div class="pagesList">
        <RouterLink 
            class="pagesListItem" 
            v-for="page in pages" 
            :key="page.name" 
            :to="page.path"
        >
            {{ page.name }}
        </RouterLink>
    </div>
</template>

<style scoped>
.pagesList {
    display: flex;
    flex-direction: column;
    gap: 16px;
}

.pagesListItem {
    text-decoration: none;
    color: #000;
    font-weight: 500;
    width: fit-content;
}
</style>