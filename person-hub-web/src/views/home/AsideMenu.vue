<template>
  <aside>
    <div class="z-20">
      <div class="flex flex-row-reverse">
        <span class="hidden h-10 w-10 pr-2 cursor-pointer close-button" @click="toggleSideBar()">
          <svg-image  icon="close-icon.svg"></svg-image>
        </span>
      </div>

      <p class="pt-10"></p>
      <p :class="['sidebar-menu-item', isRouteActive('todos-view') ? 'active' : '']" @click="navigateToRoute('todos-view')">Todo Items</p>
      <p :class="['sidebar-menu-item', isRouteActive('events-view') ? 'active' : '']" @click="navigateToRoute('events-view')">Events</p>
    </div>
  </aside>
</template>

<script lang="ts">
import { Vue } from "vue-property-decorator"
import { mapMutations } from "vuex"
import SvgImage from "@/components/SvgImage.vue"
import AppStoreConstant from "@/store/application/application-store-constant"

const AsideMenuItem = Vue.extend({
  components: {
    SvgImage,
  },
  props: {},
  methods: {
    ...mapMutations("application", {
      toggleSideBar: AppStoreConstant.MUTATIONS.toogleSidebar,
    }),
    isRouteActive: function (routeName: string) {
      return this.$route.name == routeName
    },
    navigateToRoute(routeName) {
      if (!this.isRouteActive(routeName)) {
        this.$router.push({ name: routeName })
      }
    },
  },
})

export default AsideMenuItem
</script>
<style lang="postcss" scoped>
.sidebar-menu-item {
  @apply rounded p-2 hover:bg-gray-300 cursor-pointer;
}

.active {
  @apply bg-gray-300;
}

.open-sidebar .close-button{
  @apply inline-block;
}

</style>>

